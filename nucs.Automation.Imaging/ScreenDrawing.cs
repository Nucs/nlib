using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace nucs.Automation.Imaging {
    public class ScreenDrawing {
        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public static void Draw(Rectangle rect, Color color = default(Color)) {
            if (color == default(Color)) {
                var random = new Random();
                color = Color.FromArgb((byte) random.Next(0, 255), (byte) random.Next(0, 255), (byte) random.Next(0, 255));
            }

            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            try {
                Graphics g = Graphics.FromHdc(desktopPtr);
                SolidBrush b = new SolidBrush(color);
                g.FillRectangle(b, rect);

                g.Dispose();
            } finally {
                ReleaseDC(IntPtr.Zero, desktopPtr);
            }
        }

        public static void Draw(Point p, Color color = default(Color)) {
            Draw(new Rectangle(p.X, p.Y, 1, 1), color);
        }
    }
}