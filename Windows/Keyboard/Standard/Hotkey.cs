using System;
using System.Text;
using System.Windows.Forms;

namespace nucs.Windows.Keyboard {

    public static class HotkeyExtensions {

        public static void Register(this BaseKeyboardManager keyboard, Keys key, string description) {
            Register(keyboard, key, Keys.None, description);
        }

        public static void Register(this BaseKeyboardManager keyboard, Keys key, Keys modifiers, string description) {
            keyboard.Register(key, modifiers, description);
        }
    }

    public class Hotkey {
        public Keys Key { get; private set; }
        public Keys Modifiers { get; set; }
        public string Description { get; set; }
        
        public BaseKeyboardManager KeyboardManager { get; private set; }

        public Hotkey(Keys key, string description) {
            Modifiers = Keys.None;
            Key = key;
            Description = description;
        }

        public Hotkey(Keys key, Keys modifiers, string description) {
            Modifiers = modifiers;
            Key = key;
            Description = description;
        }

        public uint VKey {
            get {
                if (Key == Keys.None || Key == Keys.NoName) return 0;
                return (uint) Key;
            }
        }

        public bool Unregister() {
            return KeyboardManager.Unregister(Description) || KeyboardManager.Unregister(Key, Modifiers);
        }

        internal static Hotkey Create(Keys key, string description) { return new Hotkey(key, description); }

        internal static Hotkey Create(Keys key, Keys modifiers, string description) { return new Hotkey(key, modifiers, description); }

        public override bool Equals(object obj) {
            if (obj.GetType() == GetType()) {
                var o = obj as Hotkey;
                return o != null && o.Key == Key && o.Modifiers == Modifiers && string.Equals(o.Description, Description);
            }
            return false;
        }

        public override string ToString() {
            var b = new StringBuilder();
            if (Modifiers.HasFlag(Keys.Control)) b.Append("C");
            if (Modifiers.HasFlag(Keys.Alt)) b.Append("A");
            if (Modifiers.HasFlag(Keys.Shift)) b.Append("S");
            if (b.Length != 0)
                b.Append('-');
            b.Append(Key);
            return b.ToString();
        }
    }
}