using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using nucs.Automation.Mirror.Helpers;
using HWND = System.IntPtr;
namespace nucs.Automation.Mirror {
    /// <summary>
    ///     Tools to find a window specific
    /// </summary>
    public class Window {
        /// <summary>
        ///     The process this window belongs to.
        /// </summary>
        public SmartProcess Process { get; set; }

        /// <summary>
        ///     The window handle (hWnd).
        /// </summary>
        public HWND Handle { get; set; }

        /// <summary>
        ///     Returns window title, if failed returns null.
        /// </summary>
        public string Title {
            get {
                try {
                    int length = GetWindowTextLength(Handle);
                    if (length == 0)
                        return string.Empty;

                    StringBuilder builder = new StringBuilder(length);
                    GetWindowText(Handle, builder, length + 1);
                    return builder.ToString();
                } catch {
                    return null;
                }
            }
        }

        /// <summary>
        ///     The process name of this window
        /// </summary>
        public string ProcessName => Process.ProcessName;

        /// <summary>
        ///     Is the handle still valid and window still exist.
        /// </summary>
        public bool IsValid => IsWindow(Handle);

        /// <summary>
        ///     Position of the window
        /// </summary>
        public Rectangle Position => GetWindowRect(Handle);

        /// <summary>
        ///     Moves the current window to the given position while maintaining the window size.
        /// </summary>
        public void SetWindowPosition(Point point) {
            SetWindowPosition(point.X,point.Y);
        }

        /// <summary>
        ///     Is the current window is at the ForegroundWindow
        /// </summary>
        public bool IsFocused => GetForegroundWindow().ToInt32() == Handle.ToInt32();

        /// <summary>
        ///     Moves the current window to the given position while maintaining the window size.
        /// </summary>
        public void SetWindowPosition(int x, int y) {
            var rect = Position;
            MoveWindow(Handle, 0, 0, rect.Width, rect.Height, true);
        }

        /// <summary>
        ///     Sets the window size to the given new size
        /// </summary>
        public void SetWindowSize(Size size) {
            SetWindowSize(size.Width, size.Height);
        }
        /// <summary>
        ///     Sets the window size to the given new size
        /// </summary>
        public void SetWindowSize(uint width, uint height) {
            SetWindowSize(Convert.ToInt32(width), Convert.ToInt32(height));
        }
        /// <summary>
        ///     Sets the window size to the given new size
        /// </summary>
        public void SetWindowSize(int width, int height) {
            var rect = Position;
            MoveWindow(Handle, rect.X, rect.Y, width, height, true);
        }

        public void ChangeWindowState(WindowState state) {
            ShowWindowAsync(Handle, (int) state);
        }

        internal Window(SmartProcess process, HWND handle) {
            Process = process;
            Handle = handle;
        }

        #region NativeWin32

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(HWND hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private static Rectangle GetWindowRect(HWND hWnd) {
            RECT rect;
            if (GetWindowRect(hWnd, out rect) == false)
                return Rectangle.Empty;
            return new Rectangle { X = rect.Left, Y = rect.Top, Width = rect.Right - rect.Left + 1, Height = rect.Bottom - rect.Top + 1 };
        }

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);
        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(HWND hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(HWND hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern HWND GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);
        #endregion

    }

    public static class WindowEx {
        /// <summary>
        ///     Gets all the windows in the smart proc
        /// </summary>
        public static List<Window> GetWindows(this SmartProcess sproc) {
            var wnds = OpenWindowGetter.GetOpenWindows();
            return null;
        }
    }
}