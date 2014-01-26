using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using nucs.Windows.Hooking;

namespace nucs.Windows.Keyboard {
    public sealed class HotKeyManager : IDisposable {
        internal delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);
        internal delegate bool ReturnBool(IntPtr handle, int id);
        public delegate void KeyPressEventHandler(HotkeyRegisteration args);
        public event KeyPressEventHandler HotKeyPressed;
        private readonly ManualResetEventSlim _windowReadyEvent = new ManualResetEventSlim(false);
        internal readonly List<HotkeyRegisteration> Registers = new List<HotkeyRegisteration>();
        public ReadOnlyCollection<HotkeyRegisteration> Registerations { get { return Registers.AsReadOnly(); } }
        private KeyboardHook Hooker { get; set; }
        public HotKeyManager() {
            Hooker = new KeyboardHook();
            Hooker.KeyUp += HookerOnKeyPress;
            
            //while (Hooker.IsStarted == false)
                //Hooker.Start(this); //todo fix!!
            
        }

        private void HookerOnKeyPress(object sender, KeyEventArgs e) {
            HotkeyRegisteration reg;
            if ((reg = Registers.FirstOrDefault(k => k.Key == e.KeyCode && k.Modifiers == (uint)e.Modifiers)) != null)
                HotKeyPressed(reg);
        }

        public bool Register(Keys key, KeyModifiers modifier, string Description) {
            if (Registers.Any(hkk => string.Equals(hkk.Description, Description) || (hkk.Modifiers == (uint)modifier && hkk.Key == key))) return false;
            var hk = HotkeyRegisteration.Create(key, new[] { modifier },this, Description);
            Registers.Add(hk);
            hk.Register();
            return true;
        }

        public bool Register(Keys key, IEnumerable<KeyModifiers> modifiers, string Description) {
            if (Registers.Any(hkk => string.Equals(hkk.Description, Description) || (hkk.Modifiers == ModifiersTools.MergeModifiers(modifiers.ToArray()) && hkk.Key == key))) return false; 
            var hk = HotkeyRegisteration.Create(key, modifiers, this, Description);
            Registers.Add(hk);
            hk.Register();
            return true;
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

        public void Dispose() {
            Clear();
            Hooker.Stop();
        }
    }

    public class HotkeyRegisteration {
        public Keys Key { get; private set; }
        public KeyModifiers[] ModifiersKeys { get; private set; }
        public KeyModifiers ModifierKeys { get { return (KeyModifiers) Modifiers; } }
        public uint Modifiers { get { return ModifiersTools.MergeModifiers(ModifiersKeys); } }
        public string Description { get; set; }
        public bool Registered { get; private set; }
        public uint VKey { get { if (Key == Keys.None || Key == Keys.NoName) return 0; return (uint) Key;}}
        private HotKeyManager Parent { get; set; }

        internal HotkeyRegisteration(Keys key, HotKeyManager Parent, string Description = "") {
            this.Parent = Parent;
            ModifiersKeys = new[] {KeyModifiers.None};
            Key = key;
            this.Description = Description;
        }

        internal HotkeyRegisteration(Keys key, IEnumerable<KeyModifiers> modifiers, HotKeyManager Parent, string Description = "") {
            this.Parent = Parent;
            ModifiersKeys = (modifiers ?? new[] {KeyModifiers.None}).Distinct().ToArray();
            Key = key;
            this.Description = Description;
        }

        public bool Register() {
            if (Registered) return false;

            return true;
        }

        public bool Unregister() {
            if (Registered == false) return false;

            return true;
        }

        internal static HotkeyRegisteration Create(Keys key, HotKeyManager Parent, string Description = "") {
            return new HotkeyRegisteration(key, Parent, Description);
        }

        internal static HotkeyRegisteration Create(Keys key, IEnumerable<KeyModifiers> modifiers, HotKeyManager Parent,  string Description = "") {
            return new HotkeyRegisteration(key, modifiers, Parent, Description);
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
}