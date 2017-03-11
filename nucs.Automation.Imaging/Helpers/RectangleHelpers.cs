using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using nucs.Automation.Mirror;

namespace nucs.Automation.Imaging.Helpers {
    public static class RectangleHelpers {
        static RectangleHelpers() {
            var s = Screen.AllScreens;
            if (s.Any(scr => scr.Bounds.Y != 0))
                throw new InvalidOperationException("Screens that are not aligned horizontally are not supported.");
            var abs = s.OrderBy(scr => scr.Bounds.X).ToArray();
            for (int i = 0; i < abs.Length; i++) {
                var b = abs[i].Bounds;
                b.X = 0;
                for (int j = 0; j < i - 1; j++) {
                    b.X += abs[j].Bounds.Width;
                }
                if (i != 0)
                    b.X += b.Width;
                RelativeScreen.Add(b, abs[i]);
            }
        }
        public static Dictionary<Rectangle, Screen> RelativeScreen = new Dictionary<Rectangle, Screen>();
        public static Rectangle RelativeToScreenshot(this Rectangle sysrelative) {
            var targetpos = sysrelative.X;
            var t = RelativeScreen.FirstOrDefault(b => targetpos >= b.Key.X && targetpos <= b.Key.X + b.Key.Width);
            var relativepos = targetpos - t.Key.X;
            var newpos = t.Value.Bounds.X + relativepos;
            sysrelative.X = newpos;
            return sysrelative;
        }
        public static Rectangle RelativeToWindow(this Rectangle relative, Window win) {
            var winpos = win.Position;
            var @out = new Rectangle(relative.X+winpos.X, relative.Y+winpos.Y, relative.Width, relative.Height);
            return @out;
        }
        public static IEnumerable<Rectangle> Split(this Rectangle pRect, int rows, int columns) {
            float width = 1f * pRect.Width / columns;
            float height = 1f * pRect.Height / rows;
              for (int c = 0; c < columns; c++)
                 for (int r = 0; r < rows; r++)
                 {
                    yield return new Rectangle((pRect.X + c * width).toInt(), (pRect.Y + r * height).toInt(), width.toInt(), height.toInt());
                    // e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                 }
/*
                for (int c = 0; c < columns; c++)
                yield return new Rectangle((pRect.X + c * width).toInt(), (pRect.Y), (pRect.X + c * width).toInt(), pRect.Y + pRect.Height);
                for (int r = 0; r < rows; r++) yield return new Rectangle(pRect.X, (pRect.Y + r * height).toInt(), pRect.X + pRect.Width, (pRect.Y + r * height).toInt());*/
        }

        private static int toInt(this float f) {
            return Convert.ToInt32(Math.Round(f));
        }
    }
}