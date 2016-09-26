using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using HWND = System.IntPtr;

namespace nucs.Automation.Mirror.Helpers {
    /// <summary>Contains functionality to get all the open windows.</summary>
    public static class OpenWindowGetter {
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static List<HWND> GetOpenWindows() {
            var shellWindow = GetShellWindow();
            var windows = new List<HWND>();

            EnumWindows(delegate(HWND hWnd, int lParam) {
                if (hWnd == shellWindow)
                    return true;
                if (!IsWindowVisible(hWnd))
                    return true;
                windows.Add(hWnd);
                return true;
            }, 0);

            return windows;
        }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();
    }
}