/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

namespace nucs.Windows.Keyboard {
    public sealed class GlobalHotkeyManager : BaseKeyboardManager {
        /*internal delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);#1#
        internal delegate bool ReturnBool(IntPtr handle, int id);
        
        private MessageWindow _wnd;
        private IntPtr _hwnd;
        private readonly ManualResetEventSlim _windowReadyEvent = new ManualResetEventSlim(false);
        private int _id = 0;

        public GlobalHotkeyManager() {
            var messageLoop = new Thread(() => Application.Run(_wnd = new MessageWindow(this, _windowReadyEvent))) { Name = "MessageLoopThread", IsBackground = true };
            messageLoop.Start();
        }
        //bug: cant bind f12, appreantly because kernel binds it, do following: http://muzso.hu/2011/12/13/setting-f12-as-a-global-hotkey-in-windows

        public override Hotkey Register(Keys key, string description) {
            return Register(key, Keys.None, description);
        }

        public Hotkey Register(AKey akey, string description) {
            var key = (Keys) akey.KeyValue;
            var modifier = (Keys) akey.Modifiers.Select(k => (uint)k).Aggregate((a, b) => a | b);
            return Register(key, modifier, description);
        }

        public override Hotkey Register(Keys key, Keys modifiers, string description) {
            if (!_windowReadyEvent.IsSet)
                _windowReadyEvent.Wait();
            if (Registers.Any(hkk => string.Equals(hkk.Description, description) || (hkk.Modifiers == modifiers && hkk.Key == key))) return null; 
            int id = System.Threading.Interlocked.Increment(ref _id);
            Hotkey hk = Hotkey.Create(key, modifiers, description);
            Registers.Add(hk);
            object res = false;
            //var handle = _wnd.Handle;

            return (bool)_wnd.Invoke(new Func<IntPtr, int, bool>(hk.Register), _hwnd, id) ? hk : null; //return  //todo fix the invoker.. it doesn't wait for it to be done and sends automatically res=false..
            //todo add invoke extension for easier access for async
        }

        public override bool Unregister(string description) {
            var hk = Registers.FirstOrDefault(k => string.Equals(description, k.Description));
            if (hk == null || hk.Registered == false) return false;
            Registers.Remove(hk);
            return hk.Unregister();
        }

        public override bool Unregister(Keys key) {
            var hk = Registers.FirstOrDefault(k => k.Key == key && k.Modifiers == (uint)Keys.None);
            if (hk == null || hk.Registered == false) return false;
            Registers.Remove(hk);
            return hk.Unregister();
        }

        public override bool Unregister(Keys key, Keys modifier) {
            var hk = Registers.FirstOrDefault(k => k.Key == key && k.Modifiers == modifier);
            if (hk == null || hk.Registered == false) return false;
            Registers.Remove(hk);
            return hk.Unregister();
        }

        public override bool UnregisterAll() {
            if (Registers.Count == 0) return false;
            Registers.ForEach(hk=> hk.Unregister());
            return true;
        }

        public override void Clear() {
            UnregisterAll();
            Registers.Clear();
        }

        private void OnHotKeyPressed(HotkeyEventArgs e) {
            if (e == null || e.Hotkey == null) return;
            InvokeHotKeyPressed(e);
        }

        private sealed class MessageWindow : Form {
            private readonly GlobalHotkeyManager _manager;
            private const int WM_HOTKEY = 0x312;
            public MessageWindow(GlobalHotkeyManager manager, ManualResetEventSlim _event) {
                _manager = manager;
                _manager._wnd = this;
                Shown += (sender, args) => Hide();
                HandleCreated += (sender, args) => { _manager._hwnd = Handle; _event.Set(); };
                if (!IsHandleCreated)
                    CreateHandle();
            }

            [DebuggerStepThrough]
            protected override void WndProc(ref Message m) {
                if (m.Msg == WM_HOTKEY) {
                    var e = HotkeyEventArgs.Create(_manager, m.LParam);
                    _manager.OnHotKeyPressed(e);
                }

                base.WndProc(ref m);
            }

            protected override void SetVisibleCore(bool value) {
                // Ensure the window never becomes visible
                try {
                    base.SetVisibleCore(false);
                } catch {}
            }

            
        }
        public void Dispose() {
            Clear();
        }
    }

}*/