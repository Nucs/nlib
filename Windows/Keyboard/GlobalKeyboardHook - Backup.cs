/*using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace nucs.Windows.Keyboard {

    public class ApplicationSimulator : Form {
        internal static ApplicationSimulator mInstance;
        public static IntPtr hInstance = IntPtr.Zero;
        public static ManualResetEventSlim Loaded = new ManualResetEventSlim(false);
        public static bool IsStarted {
            get { return mInstance != null; }
        }

        public static void Hook() {
            if (IsStarted == false)
                Start();
        }

        public static void UnHook() {
            if (IsStarted == true)
                Stop();
        }

        public static void Start() {
            var t = new Thread(runForm);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
            Loaded.Wait();
        }

        public static void Stop() {
            if (mInstance == null) throw new InvalidOperationException("Notifier not started");
            mInstance.Invoke(new MethodInvoker(mInstance.endForm));
            hInstance = IntPtr.Zero;
            GlobalHotkeyManager.unhook(hInstance);
            Loaded.Reset();
        }

        private static void runForm() { Application.Run(new ApplicationSimulator()); }
        private void endForm() { Close(); }

        protected override void SetVisibleCore(bool value) {
            // Prevent window getting visible
            if (mInstance == null) CreateHandle();
            mInstance = this;
            value = false;
            hInstance = NativeWin32.LoadLibrary("user32");
            GlobalHotkeyManager.hook(hInstance);
            base.SetVisibleCore(value);
            Loaded.Set();
        }

    }




    public delegate void DeviceNotifyDelegate(Message msg);



    public static class GlobalHotkeyManager {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;


        #region Events

        /// <summary>
        ///     Occurs when one of the hooked keys is pressed
        /// </summary>
        public static event KeyEventHandler KeyDown;

        /// <summary>
        ///     Occurs when one of the hooked keys is released
        /// </summary>
        public static event KeyEventHandler KeyUp;

        #endregion

        /// <summary>
        ///     The collections of keys to watch for
        /// </summary>
        public static List<Keys> HookedKeys = new List<Keys>();

        /// <summary>
        ///     Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        private static IntPtr Handle = IntPtr.Zero;

        public static keyboardHookProc callbackDelegate;

        public static void Start() {
            if (Console.Title != string.Empty) //TODO replace with normal check for console app
                ApplicationSimulator.Hook();
            else
                hook(NativeWin32.LoadLibrary("user32.dll"));

        }

        public static void Stop() {
            if (Console.Title != string.Empty) //TODO replace with normal check for console app
                ApplicationSimulator.UnHook();
            else
                unhook(Handle);
        }

        public static void hook(IntPtr hInstance) {
            callbackDelegate = new keyboardHookProc(hookProc);
            GC.KeepAlive(callbackDelegate);
            Handle = NativeWin32.SetWindowsHookEx(WH_KEYBOARD_LL, callbackDelegate, hInstance, 0);
        }

        /// <summary>
        ///     Uninstalls the global hook
        /// </summary>
        public static void unhook(IntPtr hhook) {
            NativeWin32.UnhookWindowsHookEx(hhook);
        }

        /// <summary>
        ///     The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public static int hookProc(int code, int wParam, ref keyboardHookStruct lParam) {
            if (code >= 0) {
                var key = (Keys) lParam.vkCode;
                if (HookedKeys.Contains(key)) {
                    var kea = new KeyEventArgs(key);
                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null)) {
                        KeyDown(null, kea);
                    } else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null)) {
                        KeyUp(null, kea);
                    }
                    if (kea.Handled)
                        return 1;
                }
            }
            return NativeWin32.CallNextHookEx(Handle, code, wParam, ref lParam);
        }

    }
}*/