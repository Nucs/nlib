using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using nucs.Automation.Imaging.Helpers;
using nucs.Summoners;
using nucs.Threading;
using nucs.Threading.FastThreadPool;
using nucs.Threading.Syncers;
using Nito.AsyncEx;

namespace nucs.Automation.Imaging {
    public partial class DirectBitmap : IDisposable, IEquatable<DirectBitmap> {
        public void Save(FileInfo file, ImageFormat format = null) {
            Save(file.FullName, format);
        }

        public void Save(string filename, ImageFormat format = null) {
            Bitmap.Save(filename, format ?? ImageFormat.Png);
        }

        /// <summary>
        ///     Find images on the screen
        /// </summary>
        /// <param name="small"></param>
        /// <param name="tolerance_precentage"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static List<Rectangle> FindOnScreen(DirectBitmap small, double tolerance_precentage, ImageSearchingOptions opts = null) {
            if (opts != null)
                opts.IsSearchingInAllScreens = true;
            else
                opts = ImageSearchingOptions.Screenshot;

            return FromScreenshot().FindSubimages(small, tolerance_precentage, opts);
        }

        /// <summary>
        ///     Searches for a bitmap inside a larger one
        /// </summary>
        /// <param name="small">Image (Bitmap) to look for </param>
        /// <param name="tolerance_precentage"></param>
        /// <param name="opts"></param>
        /// <returns>
        ///     True if image was found, and changes the Point value passed to it to the top left co-ordinates of where the
        ///     image was found
        /// </returns>
        public List<Rectangle> FindSubimages(DirectBitmap small, double tolerance_precentage, ImageSearchingOptions opts = null) {
            opts = opts ?? ImageSearchingOptions.Default;
            List<Rectangle> @out = null;
            byte margin = Convert.ToByte(tolerance_precentage == 0d ? 0 : Convert.ToInt32(Math.Round(255d * (tolerance_precentage / 100d))));

            if (opts.AllowMultithreading) {
                @out = _findSubimagesMultithreaded(small, margin, opts).Result;
            } else {
                @out = _findSubimages(small, margin, opts);
            }

            //////////////////////////////////////
            /// 
            if (opts.IsSearchingInAllScreens && @out != null && @out.Count > 0)
                for (int i = 0; i < @out.Count; i++) {
                    @out[i] = @out[i].RelativeToScreenshot();
                }

            return @out;
        }

        protected async Task<List<Rectangle>> _findSubimagesMultithreaded(DirectBitmap small, byte margin, ImageSearchingOptions opts) {
            var xy = MathHelper.CalculateTableForGrids(opts.Threads);
            var rects = this.Area.Split(xy.X, xy.Y).ToArray();
            var token = new cancelToken();
            DataflowNotifier<List<Rectangle>> df=null;
            AsyncManualResetEvent res = new AsyncManualResetEvent(false);
            if (opts.NumberOfResults > 0) { //limit search!
                df = new DataflowNotifier<List<Rectangle>>(r => {
                    var @out = !(df.Collection.Count >= opts.NumberOfResults);
                    if (@out == false) { //has reached limit
                        token.Cancel();
                        res.Set();
                    }
                    return @out;
                }, true);
            }
            else 
            df = new DataflowNotifier<List<Rectangle>>(true);
            Debugging.Restart();
            //Debugging.Print("Ready to fire");
            int i = 0;
            Task[] tasks = opts.IgnoreAlphaPixels && small.Map!=null 
                ? rects.Select(r => Pool.Run(() => _findImageAlpha(small, r, small.Map.Ranges, margin, token, opts)).AndThen(t => df.Set(t))).ToArray()
                : rects.Select(r => Pool.Run(() => _findImage(small, r, margin, token, opts)).AndThen(t => df.Set(t))).ToArray();
            await Task.WhenAny(Task.WhenAll(tasks), res.WaitAsync());
            //Debugging.Print("Has Ended");
            return df.Collection.ToArray().SelectMany(l => l).ToList();
        }

        protected List<Rectangle> _findSubimages(DirectBitmap small, byte margin, ImageSearchingOptions opts) {
            //TODO alpha specific
            Debugging.Restart();
            //Debugging.Print("Ready to fire");
            if (small.Map != null && opts.IgnoreAlphaPixels) {
                if (small.Map.Ranges.Count == 0)
                    return new List<Rectangle>(0);
                return _findImageAlpha(small, this.Area, small.Map.Ranges, margin, new cancelToken(), opts);
            }
            return _findImage(small, this.Area, margin, new cancelToken(), opts);
        }

        protected virtual unsafe List<Rectangle> _findImageAlpha(DirectBitmap small, Rectangle area, List<SearchRange> ranges, int margin, cancelToken token, ImageSearchingOptions opts) {
            opts = opts ?? ImageSearchingOptions.Default;
            var @out = new List<Rectangle>(0);
            int jump_small = small.PixelSize;
            int jump_large = this.PixelSize;

            byte* scan_large = (byte*) this._scan0; //[largeX+ smallX, largeY+ smallY];
            byte* scan_small = (byte*) small._scan0; //[smallX, smallY];
            int c, smallX = 0, smallY = 0, pix = 0;
            byte* s, l;
            //Debugging.Print($"{Thread.CurrentThread.ManagedThreadId} has started");
            for (int largeY = area.Y; largeY < area.Y + area.Height; largeY++) {
                if (token.cancel) //here so it wont impact performance so bad
                    return @out;
                for (int largeX = area.X; largeX < area.X + area.Width; largeX++) {

                    foreach (var range in ranges) {
                        for (smallY = range.StartY; smallY <= range.StopY; smallY++) {
                            for (smallX = range.StartX; smallX <= range.StopX; smallX++) {
                                l = scan_large + (((largeX + smallX) + (largeY + smallY) * this.Width) * jump_large);
                                s = scan_small + (smallX + smallY * small.Width) * jump_small;
                                for (pix = 0; pix < 3; pix++) {
                                    c = l[pix] - s[pix]; //c==margin
                                    if (c > margin || c < -margin)
                                        goto nextLoop;
                                }
                            }
                        }
                    }


                    //If all the pixels match up, then return true and change Point location to the top left co-ordinates where it was found
                    @out.Add(new Rectangle(largeX, largeY, small.Width, small.Height));
                    Debugging.Print($"{Thread.CurrentThread.ManagedThreadId} has found: {new Rectangle(largeX, largeY, small.Width, small.Height)}");
                    if (opts.NumberOfResults != -1 && @out.Count >= opts.NumberOfResults)
                        return @out;
                    //Go to next pixel on large image
                    nextLoop:
                    ;
                }
            }
            //Return false if image is not found, and set an empty point
            /*            location = Point.Empty;*/
            //Debugging.Print($"{Thread.CurrentThread.ManagedThreadId} has finished");
            return @out;
        }

        protected virtual unsafe List<Rectangle> _findImage(DirectBitmap small, Rectangle area, int margin, cancelToken token, ImageSearchingOptions opts) {
            opts = opts ?? ImageSearchingOptions.Default;
            var @out = new List<Rectangle>(0);
            int jump_small = small.PixelSize;
            int jump_large = this.PixelSize;

            byte* scan_large = (byte*) this._scan0; //[largeX+ smallX, largeY+ smallY];
            byte* scan_small = (byte*) small._scan0; //[smallX, smallY];
            int c, smallX = 0, smallY = 0, pix = 0;
            byte* s, l;
            //Debugging.Print($"{Thread.CurrentThread.ManagedThreadId} has started");
            for (int largeY = area.Y; largeY < area.Y + area.Height; largeY++) {
                if (token.cancel) //here so it wont impact performance so bad
                    return @out;
                for (int largeX = area.X; largeX < area.X + area.Width; largeX++) {
                    for (smallY = 0; smallY < small.Height; smallY++) {
                        for (smallX = 0; smallX < small.Width; smallX++) {
                            l = scan_large + (((largeX + smallX) + (largeY + smallY) * this.Width) * jump_large);
                            s = scan_small + (smallX + smallY * small.Width) * jump_small;
                            for (pix = 0; pix < 3; pix++) {
                                c = l[pix] - s[pix]; //c==margin
                                if (c > margin || c < -margin)
                                    goto nextLoop;
                            }
                        }
                    }
                    //If all the pixels match up, then return true and change Point location to the top left co-ordinates where it was found
                    @out.Add(new Rectangle(largeX, largeY, small.Width, small.Height));
                    Debugging.Print($"{Thread.CurrentThread.ManagedThreadId} has found: {new Rectangle(largeX, largeY, small.Width, small.Height)}");
                    if (opts.NumberOfResults != -1 && @out.Count >= opts.NumberOfResults)
                        return @out;
                    //Go to next pixel on large image
                    nextLoop:
                    ;
                }
            }
            //Return false if image is not found, and set an empty point
/*            location = Point.Empty;*/
            //Debugging.Print($"{Thread.CurrentThread.ManagedThreadId} has finished");
            return @out;
        }

        [StructLayout(LayoutKind.Sequential)]
        

        protected class cancelToken {
            /// <summary>
            ///     If true, meaing a cancel has been requested
            /// </summary>
            public bool cancel { get; private set; } = false;

            public void Cancel() {
                cancel = true;
            }
        }
    }

    [DebuggerDisplay("{R}:{G}:{B}:{A}")]
    public struct pixel {
        public byte B;
        public byte G;
        public byte R;
        public byte A;

        public bool isTransparent => A == 0;

        public override string ToString() {
            return $"{R}:{G}:{B}:{A}";
        }
    }
}