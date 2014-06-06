using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using nucs.Forms;

namespace nucs.Windows.Keyboard {
    

    public class ApplicationSimulator : InvisibleForm {
        public IntPtr hInstance = IntPtr.Zero;

        public delegate void MessageArrivalHandler(ref Message msg);

        /// <summary>
        /// Provides access to the messages that arrives through the application, unfiltered.
        /// </summary>
        public event MessageArrivalHandler MessageArrived; 
        private ApplicationSimulator() { }

        private ApplicationSimulator(Action<InvisibleForm> post_created) : base(post_created) {}

        
        public void Invoke(Action act) {
            base.Invoke(act);
        }

        protected override void WndProc(ref Message m) {
            if (MessageArrived != null)
                MessageArrived(ref m);
            base.WndProc(ref m);
        }

        public static ApplicationSimulator Start() {
            ApplicationSimulator instace_holder = null;

            var holder = new ManualResetEventSlim(false);
            var t = new Thread(()=> Application.Run(
                instace_holder = new ApplicationSimulator((inst) => {
                    instace_holder = (ApplicationSimulator)inst; holder.Set();
                }
            )));

            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
            holder.Wait(); //wait for init.
            return instace_holder;
        }
    }


    /*public class ApplicationSimulator
    {
        public IntPtr hInstance = IntPtr.Zero;
        public ManualResetEventSlim Loaded = new ManualResetEventSlim(false);

        public bool IsStarted
        {
            get { return Form != null; }
        }

        public InvisibleForm Form { get; private set; }


        public ApplicationSimulator() { }

        public virtual void PostCreated() { }

        /*        public void Hook() {
                    if (IsStarted == false)
                        Start();
                }

                public void UnHook() {
                    if (IsStarted == true)
                        Stop();
                }#1#

        public virtual void Start()
        {
            if (IsStarted)
                return;
            var t = new Thread(runForm);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
            Loaded.Wait();
        }

        /* #1#
        public virtual void Stop()
        {
            if (!IsStarted)
                return;
            frm.Invoke(new MethodInvoker(endForm));
            hInstance = IntPtr.Zero;
            GlobalHotkeyManager.unhook(hInstance);
            Loaded.Reset();
        }

        public void Invoke(Action act)
        {
            Form.Invoke(act);
        }

        private void runForm() { Application.Run(Form = new InvisibleForm(form => PostCreated())); }
        private void endForm() { Form.Close(); }


    }*/


    /*GlobalHotkeyManager.hook(hInstance);
            hInstance = NativeWin32.LoadLibrary("user32");
            Loaded.Set();*/



    public delegate void DeviceNotifyDelegate(Message msg);



    public class GlobalHotkeyManager : BaseKeyboardManager {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        private readonly bool IsConsole;
        private ApplicationSimulator app;
        public GlobalHotkeyManager(bool IsConsoleApplication = false) {
            if ((IsConsole = IsConsoleApplication) == true)
                app = ApplicationSimulator.Start();
        }


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

        public override bool IsStarted { get; protected set; }

        public override void Start() {
            if (IsStarted) 
                return;
            if (IsConsole)
                app.Start();
            else
                hook(NativeWin32.LoadLibrary("user32.dll"));
            IsStarted = true;
        }

        public override void Stop() {
            if (IsStarted == false) 
                return;
            if (IsConsole)
                app.Stop();
            else
                unhook(Handle);
            IsStarted = false;
        }

        internal static void hook(IntPtr hInstance) {
            callbackDelegate = new keyboardHookProc(hookProc);
            GC.KeepAlive(callbackDelegate);
            Handle = NativeWin32.SetWindowsHookEx(WH_KEYBOARD_LL, callbackDelegate, hInstance, 0);
        }

        /// <summary>
        ///     Uninstalls the global hook
        /// </summary>
        internal static void unhook(IntPtr hhook) {
            NativeWin32.UnhookWindowsHookEx(hhook);
        }

        /// <summary>
        ///     The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        internal static int hookProc(int code, int wParam, ref keyboardHookStruct lParam) {
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


        public override bool Unregister(string description) {
            return _registers.RemoveWhere(hk => hk.Description.Equals(description)) > 0;
        }

        public override bool Unregister(Keys key, Keys modifier) {
            return _registers.RemoveWhere(hk => hk.Key == key && hk.Modifiers == modifier) > 0;
        }

        public override bool UnregisterAll() {
            _registers.Clear();
            return true;
        }

    }
}