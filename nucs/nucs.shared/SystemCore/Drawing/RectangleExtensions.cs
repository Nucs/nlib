
#if !NET4
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


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
    }
}
#endif