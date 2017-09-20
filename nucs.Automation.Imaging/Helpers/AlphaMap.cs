using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using nucs.SystemCore.AttachedObjects;

namespace nucs.Automation.Imaging.Helpers {

    /// <summary>
    ///     A class that maps Bitmap's areas that are not transparent upon construction.
    /// </summary>
    public class AlphaMap : IEnumerable<SearchRange> {
        public List<SearchRange> Ranges { get; private set; }

        public AlphaMap(DirectBitmap bitmap) {
            Ranges = GetRange(bitmap);
        }

        private List<SearchRange> GetRange(DirectBitmap bitmap) {
            var ranges = new List<SearchRange>(0);
            if (bitmap.Format.ToString().Contains("A", StringComparison.Ordinal) == false) {
                ranges.Add(new SearchRange(0, 0, bitmap.Width - 1, bitmap.Height - 1));
                return ranges;
            }

            //map sequential ranges of solid items
            var map = transparentseqmap(bitmap);
            int pointer = 0;
            bool istransparent = true;
            //skip till first occurence
            while (map.Length > pointer && map[pointer] == istransparent) {
                pointer++;
            }
            if (pointer >= map.Length) { //all transparent
                return ranges; //return.
            }

            //start collecting
            var points = new List<Point>();
            while (true) {
                var p = new Point(pointer, -1);
                while (map.Length > pointer && map[pointer] == !istransparent) {
                    pointer++;
                }
                if (map.Length > pointer == false) { //has reached end
                    p.Y = map.Length - 1;
                    points.Add(p);
                    break;
                }
                p.Y = pointer - 1;
                points.Add(p);
                while (map.Length > pointer && map[pointer] == istransparent) { //skip while transparent
                    pointer++;
                }
                if (map.Length > pointer == false) { //has reached end
                    break;
                }
            }

            //map sequential to SearchRanges
            var width = bitmap.Width;
            foreach (var range in points) {
                var linear1 = range.X;
                var linear2 = range.Y;
                var r = new SearchRange(linear1 % width, linear1 / width, linear2 % width, linear2 / width);
                ranges.Add(r);
            }
            return ranges;
        }

        private unsafe bool[,] transparentmap(DirectBitmap bm) {
            //search em
            int jump_large = 4;
            byte* scan_large = (byte*) bm.Scan0.ToPointer();
            int c, pix = 0;
            pixel* l;

            bool[,] map = new bool[bm.Width, bm.Height];

            for (int y = 0; y < bm.Height; y++) {
                for (int x = 0; x < bm.Width; x++) {
                    l = (pixel*) (scan_large + (((x) + (y) * bm.Width) * jump_large));
                    map[x, y] = l->isTransparent;
                }
            }
            return map;
        }

        private unsafe bool[] transparentseqmap(DirectBitmap bm) {
            //search em
            int jump_large = 4;
            byte* scan_large = (byte*) bm.Scan0.ToPointer();
            int c, pix = 0;
            pixel* l;

            bool[] map = new bool[bm.Width * bm.Height];

            for (int y = 0; y < bm.Height; y++) {
                for (int x = 0; x < bm.Width; x++) {
                    l = (pixel*) (scan_large + (((x) + (y) * bm.Width) * jump_large));
                    map[x + y * bm.Width] = l->isTransparent;
                }
            }
            return map;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<SearchRange> GetEnumerator() {
            return Ranges.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }



    /// <summary>
    ///     Represents a range that needs to be searched
    /// </summary>
    [DebuggerStepThrough]
    [DebuggerDisplay("{Start} -> {Stop}")]
    public class SearchRange {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int StopX { get; set; }
        public int StopY { get; set; }
        public Point Start => new Point(StartX, StartY);
        public Point Stop => new Point(StopX, StopY);

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public SearchRange(int startX, int startY, int stopX, int stopY) {
            StartX = startX;
            StartY = startY;
            StopX = stopX;
            StopY = stopY;
        }

        public SearchRange(Point start, Point stop) {
            StartX = start.X;
            StartY = start.Y;
            StopX = stop.X;
            StopY = stop.Y;
        }
    }
}