using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using nucs.Threading.FastThreadPool;

namespace nucs.Automation.Imaging {
    public partial class DirectBitmap : IDisposable, IEquatable<DirectBitmap> {
        
        static DirectBitmap() {
            Task.Run(() => Pool);
        }

        private static readonly object _cache_sync = new object();
        private static FastThreadPool _default = null;

        /// <summary>
        ///     The Default FastThreadPool that is used by the current application
        /// </summary>
        public static FastThreadPool Pool {
            get {
                if (_default != null)
                    return _default;
                lock (_cache_sync) {
                    if (_default == null)
                        return _default = new FastThreadPool(4);
                }
                return _default;
            }
        }

        public static DirectBitmap FromScreenshot() {
            var dbmp = new DirectBitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height, PixelFormat.Format24bppRgb);
            using (var screenGraph = Graphics.FromImage(dbmp.Bitmap)) {

                screenGraph.CopyFromScreen(SystemInformation.VirtualScreen.X,
                    SystemInformation.VirtualScreen.Y,
                    0,
                    0,
                    SystemInformation.VirtualScreen.Size,
                    CopyPixelOperation.SourceCopy);
            }
            return dbmp;
        }

        public static DirectBitmap FromScreenshot(Screen screen) {
            var dbmp = new DirectBitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format24bppRgb);
            using (var screenGraph = Graphics.FromImage(dbmp.Bitmap)) {
                screenGraph.CopyFromScreen(screen.WorkingArea.X,
                    screen.WorkingArea.Y,
                    0,
                    0,
                    screen.WorkingArea.Size,
                    CopyPixelOperation.SourceCopy);
            }
            return dbmp;
        }

        /// <summary>
        ///     Loads a DirectBitmap from a file into memory.
        /// </summary>
        /// <param name="file">The file to load</param>
        /// <param name="calculateAlphaMap">Should the alphamap be loaded?</param>
        public static DirectBitmap FromFile(FileInfo file, bool calculateAlphaMap = false) {
            return FromFile(file.FullName, calculateAlphaMap);
        }

        /// <summary>
        ///     Loads a DirectBitmap from a file into memory.
        /// </summary>
        /// <param name="file">The file to load</param>
        /// <param name="calculateAlphaMap">Should the alphamap be loaded?</param>
        public static DirectBitmap FromFile(string file, bool calculateAlphaMap = false) {
            using (var bmp = Image.FromFile(file)) {
                if (calculateAlphaMap)
                    return new AlphaDirectBitmap(bmp as Bitmap ?? new Bitmap(bmp));
                return new DirectBitmap(bmp as Bitmap ?? new Bitmap(bmp));
            }
        }
    }
}