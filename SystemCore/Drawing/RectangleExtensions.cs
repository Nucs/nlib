using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.SystemCore
{
    public static class RectangleExtensions {
        public static Point SouthEastPoint(this Rectangle rect) {
            return new Point(rect.Right, rect.Bottom);
        }

        public static Point SouthWestPoint(this Rectangle rect) {
            return new Point(rect.Left, rect.Bottom);
        }


        public static Point WestPoint(this Rectangle rect) {
            return new Point(rect.Left, rect.Y+rect.Height/2);
        }

        public static Point EastPoint(this Rectangle rect) {
            return new Point(rect.Right, rect.Y+rect.Height/2);
        }

        public static Point NorthPoint(this Rectangle rect) {
            return new Point(rect.Left+rect.Width/2, rect.Y);
        }

        public static Point SouthPoint(this Rectangle rect) {
            return new Point(rect.Left+rect.Width/2, rect.Bottom);
        }

        public static Point Center(this Rectangle rect) {
            return new Point(rect.Left+rect.Width/2, rect.Top + rect.Height/2);
        }

        /// <summary>
        /// Creates a new rectangle from the output of <see cref="Rectangle.ToString"/>. If parsing failed, returns <see cref="Rectangle.Empty"/>.
        /// </summary>
        public static Rectangle FromString(string str) {
            try {
                if (string.IsNullOrEmpty(str)) return Rectangle.Empty;
                var rect = new RectangleConverter().ConvertFromString(str);
                if (rect == null) return Rectangle.Empty;

                return (Rectangle) rect;
            } catch {
                return Rectangle.Empty;
            }
        }

    }
}
