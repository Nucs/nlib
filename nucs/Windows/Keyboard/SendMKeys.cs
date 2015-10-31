#if !AV_SAFE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using nucs.SystemCore.String;
using nucs.Windows.ConsoleUtils;
using nucs.Windows.Keyboard;
using nucs.Windows.Processes;
using Z.ExtensionMethods.Object.Reflection;
namespace nucs.Windows.Keyboard {
/// <summary>
    /// Alias for SendMessage Key. Used to send string and key pressed to inactive window.
    /// </summary>
    public static class SendMKeys {
        #region WINAPI
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_SETTEXT = 0x0C;

        [DllImport("User32.dll")]
        private static extern IntPtr FindWindow(string strClassName, string strWindowName);


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, IntPtr Msg, IntPtr wParam, string lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, IntPtr Msg, int wParam, int lParam);

        private const int WM_GETTEXTLENGTH = 0x000E;

        private const int EM_SETSEL = 0x00B1;

        private const int EM_REPLACESEL = 0x00C2;

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private extern static int SendMessageGetTextLength(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, int lParam);
        #endregion

        /// <summary>
        /// Sends and replaces with a string the current active editing area without the window being active
        /// </summary>
        public static void SendSetString(this Process p, string text) {
            if (p == null)
                throw new ArgumentNullException("p");
            IntPtr notepadHwnd = p.MainWindowHandle;
            IntPtr editHwnd = FindWindowEx(notepadHwnd, (IntPtr)0, "Edit", null);
            Console.WriteLine("Edit finder error: "+Marshal.GetLastWin32Error());
            SendMessage(editHwnd, (IntPtr)WM_SETTEXT, (IntPtr)0, text);
        }

        
        /// <summary>
        /// Sends and replaces with a string the current active editing area without the window being active
        /// </summary>
        public static void SendSetString(this ProcessInfo p, string text) {
            if (p == null)
                throw new ArgumentNullException("p");
            p.ToProcess().SendSetString(text);
        }

        /// <summary>
        /// Sends a key to a process without it being active
        /// </summary>
        public static void SendKey(this ProcessInfo p, KeyCode kc ,SendKeyType kt) {
            if (p == null)
                throw new ArgumentNullException("p");
            p.ToProcess().SendKey(kc, kt);
        }

        /// <summary>
        /// Sends a key to a process without it being active
        /// </summary>
        public static void SendKey(this Process p, KeyCode kc,SendKeyType kt) {
            if (p==null)
                throw new ArgumentNullException("p");
            IntPtr editHwnd = FindWindowEx(p.MainWindowHandle, (IntPtr)0, "Edit", null);

            switch (kt) {
                case SendKeyType.KeyUp:
                    PostMessage(editHwnd, new IntPtr(WM_KEYUP), (int)kc, 0);
                    break;
                case SendKeyType.KeyDown:
                    PostMessage(editHwnd, new IntPtr(WM_KEYDOWN), (int)kc, 0);
                    break;
                case SendKeyType.KeyPress:
                    PostMessage(editHwnd, new IntPtr(WM_KEYDOWN), (int)kc, 0);
                    Thread.Sleep(50);
                    PostMessage(editHwnd, new IntPtr(WM_KEYUP), (int)kc, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("kt");
            }

        }


        /// <summary>
        /// Sends and appends a string the current active editing area without the window being active
        /// </summary>
        public static void SendAddString(this ProcessInfo p, string text) {
            if (p == null)
                throw new ArgumentNullException("p");
            p.ToProcess().SendAddString(text);
        }

        /// <summary>
        /// Sends and appends a string the current active editing area without the window being active
        /// </summary>
        public static void SendAddString(this Process p , string text) {
            if (p == null)
                throw new ArgumentNullException("p");
            var editBox = FindWindowEx(p.MainWindowHandle, new IntPtr(0), "Edit", null);
            var length = SendMessageGetTextLength(editBox, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            SendMessage(editBox, EM_SETSEL, length, length);
            SendMessage(editBox, EM_REPLACESEL, 1, text);
        } 

    }

    public enum SendKeyType {
        KeyUp,
        KeyDown,
        KeyPress
    }
}

#endif