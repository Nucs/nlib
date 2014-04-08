using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

namespace nucs.Windows.Keyboard {
    public sealed class GlobalHotkeyManager : IDisposable {
        /*internal delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);*/
        internal delegate bool ReturnBool(IntPtr handle, int id);
        public delegate void KeyPressEventHandler(HotKeyEventArgs args);
        public event KeyPressEventHandler HotKeyPressed;
        private MessageWindow _wnd;
        private IntPtr _hwnd;
        private readonly ManualResetEventSlim _windowReadyEvent = new ManualResetEventSlim(false);
        internal readonly List<Hotkey> Registers = new List<Hotkey>();
        public IReadOnlyCollection<Hotkey> Registerations { get { return Registers.AsReadOnly(); } } 
        private int _id = 0;

        public GlobalHotkeyManager() {
            var messageLoop = new Thread(() => Application.Run(_wnd = new MessageWindow(this, _windowReadyEvent))) { Name = "MessageLoopThread", IsBackground = true };
            messageLoop.Start();
        }
        //bug: cant bind f12, appreantly because kernel binds it, do following: http://muzso.hu/2011/12/13/setting-f12-as-a-global-hotkey-in-windows

        public bool Register(Keys key, KeyModifiers modifier, string Description) {
            if (!_windowReadyEvent.IsSet)
                _windowReadyEvent.Wait();
            if (Registers.Any(hkk => string.Equals(hkk.Description, Description) || (hkk.Modifiers == (uint)modifier && hkk.Key == key))) return false; 
            int id = System.Threading.Interlocked.Increment(ref _id);
            Hotkey hk = Hotkey.Create(key, new [] {modifier}, Description);
            Registers.Add(hk);
            bool res = false;
            _wnd.Invoke(new MethodInvoker(() => res = hk.Register(_wnd.Handle, id)));
            return res;
        }

        public bool Register(Keys key, IEnumerable<KeyModifiers> modifiers, string Description) {
            if (!_windowReadyEvent.IsSet)
                _windowReadyEvent.Wait();
            if (Registers.Any(hkk => string.Equals(hkk.Description, Description) || (hkk.Modifiers == ModifiersTools.MergeModifiers(modifiers.ToArray()) && hkk.Key == key))) return false; 
            int id = System.Threading.Interlocked.Increment(ref _id);
            Hotkey hk = Hotkey.Create(key, modifiers, Description);
            Registers.Add(hk);
            object res = false;
            //var handle = _wnd.Handle;
            return (bool)_wnd.Invoke(new Func<IntPtr, int, bool>(hk.Register), _hwnd, id); //todo fix the invoker.. it doesn't wait for it to be done and sends automatically res=false..
            //todo add invoke extension for easier access for async
        }

        public bool Unregister(string Description) {
            var hk = Registers.FirstOrDefault(k => string.Equals(Description, k.Description));
            if (hk == null || hk.Registered == false) return false;
            Registers.Remove(hk);
            return hk.Unregister();
        }

        public bool Unregister(Keys key) {
            var hk = Registers.FirstOrDefault(k => k.Key == key && k.Modifiers == (uint)KeyModifiers.None);
            if (hk == null || hk.Registered == false) return false;
            Registers.Remove(hk);
            return hk.Unregister();
        }

        public bool Unregister(Keys key, KeyModifiers modifier) {
            var hk = Registers.FirstOrDefault(k => k.Key == key && k.Modifiers == (uint)modifier);
            if (hk == null || hk.Registered == false) return false;
            Registers.Remove(hk);
            return hk.Unregister();
        }

        public bool Unregister(Keys key, KeyModifiers[] modifiers) {
            var merged = ModifiersTools.MergeModifiers(modifiers);
            var hk = Registers.FirstOrDefault(k => k.Key == key && k.Modifiers == merged);
            if (hk == null || hk.Registered == false) return false;
            Registers.Remove(hk);
            return hk.Unregister();
        }

        public bool UnregisterAll() {
            if (Registers.Count == 0) return false;
            Registers.ForEach(hk=> hk.Unregister());
            return true;
        }

        public void Clear() {
            UnregisterAll();
            Registers.Clear();
        }

        private void OnHotKeyPressed(HotKeyEventArgs e) {
            if (e == null || e.Hotkey == null) return;
            if (HotKeyPressed != null)
                HotKeyPressed(e);
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
                    var e = HotKeyEventArgs.Create(_manager, m.LParam);
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

    public class HotKeyEventArgs {
        public Hotkey Hotkey { get; private set; }
        public Keys Key { get { return Hotkey.Key; } }
        public KeyModifiers ModifierKeys { get { return Hotkey.ModifierKeys; } }
        public KeyModifiers[] ModifiersKeys { get { return Hotkey.ModifiersKeys; } }
        public uint Modifiers { get { return Hotkey.Modifiers; } }
        public string Description { get { return Hotkey.Description; } }
        public uint VKey { get { return Hotkey.VKey; } }
        
        private HotKeyEventArgs() {}

        public bool Unregister() {
            return Hotkey.Unregister();
        }

        internal static HotKeyEventArgs Create(GlobalHotkeyManager manager, IntPtr hotKeyParam) {
            var param = (uint) hotKeyParam.ToInt64();
            var key = (Keys) ((param & 0xffff0000) >> 16);
            var modifiers = (KeyModifiers) (param & 0x0000ffff);

            var hke = new HotKeyEventArgs();
            hke.Hotkey = manager.Registers.FirstOrDefault(hk => hk.Key == key && ((hk.Modifiers & 0xf) == (uint) modifiers));
            return hke.Hotkey == null ? null : hke;
        }
    }

    public class Hotkey {
        public Keys Key { get; private set; }
        public KeyModifiers[] ModifiersKeys { get; private set; }
        public KeyModifiers ModifierKeys { get { return (KeyModifiers) Modifiers; } }
        public uint Modifiers { get { return ModifiersTools.MergeModifiers(ModifiersKeys); } }
        public string Description { get; set; }
        public bool Registered { get; private set; }
        public uint VKey { get { if (Key == Keys.None || Key == Keys.NoName) return 0; return (uint) Key;}}

        internal Hotkey(Keys key, string Description = "") {
            ModifiersKeys = new[] {KeyModifiers.None};
            Key = key;
            this.Description = Description;
        }

        internal Hotkey(Keys key, IEnumerable<KeyModifiers> modifiers, string Description = "") {
            ModifiersKeys = (modifiers ?? new[] {KeyModifiers.None}).Distinct().ToArray();
            Key = key;
            this.Description = Description;
        }

        private IntPtr _handle;
        private int _id;
        public bool Register(IntPtr handle, int id) {
            if (Registered) return false;
            try {
                return Registered = NativeWin32.RegisterHotKey(_handle = handle, _id = id, Modifiers, VKey);
            } catch {
                return Registered = false;
            }
        }

        public bool Unregister() {
            if (Registered == false) return false;
            try {
                return Registered = NativeWin32.UnregisterHotKey(_handle, _id);
            } catch {
                return Registered = false;
            }
        }

        internal static Hotkey Create(Keys key, string Description = "") {
            return new Hotkey(key, Description);
        }

        internal static Hotkey Create(Keys key, IEnumerable<KeyModifiers> modifiers, string Description = "") {
            return new Hotkey(key, modifiers, Description);
        }

        public override bool Equals(object obj) {
 	        if (obj.GetType() == this.GetType()) {
 	            var o = obj as Hotkey;
 	            return o != null && o.Key == Key && o.Modifiers == Modifiers && string.Equals(o.Description,Description);
 	        }
            return false;
        }

        public override string ToString() {
            var b = new StringBuilder();
            if (ModifiersKeys.Contains(KeyModifiers.Control)) b.Append("C");
            if (ModifiersKeys.Contains(KeyModifiers.Alt)) b.Append("A");
            if (ModifiersKeys.Contains(KeyModifiers.Shift)) b.Append("S");
            if (ModifiersKeys.Contains(KeyModifiers.Windows)) b.Append("W");
            if (ModifiersKeys.Contains(KeyModifiers.Windows)) b.Append("N");
            if (b.Length != 0)
                b.Append('-');
            b.Append(Key.ToString());
            return b.ToString();
        }
    }

    [Flags]
    public enum KeyModifiers {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8,
        NoRepeat = 0x4000
    }
}