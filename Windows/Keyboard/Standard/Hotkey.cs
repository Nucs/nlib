#if !AV_SAFE
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#if (NET_3_5 || NET_3_0 || NET_2_0)
using nucs.SystemCore.Enums;
#endif
namespace nucs.Windows.Keyboard {
    public class Hotkey {
        public Keys Key { get; private set; }
        public ModKeys Modifiers { get; set; }
        public string Description { get; set; }
        
        public BaseKeyboardManager KeyboardManager { get; private set; }

        public Hotkey(Keys key, string description) {
            Modifiers = ModKeys.None;
            Key = key;
            Description = description;
        }

        public Hotkey(Keys key, ModKeys modifiers, string description) {
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
            return KeyboardManager.Unregister(Description) > 0 || KeyboardManager.Unregister(Key, Modifiers) > 0;
        }

        internal static Hotkey Create(AKey k, string description) {
            return new Hotkey(
                    k.Key.ToKeys()
                    , k.Modifiers == null ? ModKeys.None: k.Modifiers.Select(m => m.ToModKeys()).Aggregate((a, b) => a |= b)
                    , description
                );
        }
        internal static Hotkey Create(Keys key, string description) { return new Hotkey(key, description); }

        internal static Hotkey Create(Keys key, ModKeys modifiers, string description) { return new Hotkey(key, modifiers, description); }

        public override bool Equals(object obj) {
            if (obj.GetType() == GetType()) {
                var o = obj as Hotkey;
                return o != null && o.Key == Key && o.Modifiers == Modifiers && string.Equals(o.Description, Description);
            }
            return false;
        }

        public override int GetHashCode() {
            return ((int)this.Key)^((int)this.VKey)^(int) Modifiers ^ Description?.GetHashCode() ?? "".GetHashCode();
        }

        public override string ToString() {
            var b = new StringBuilder();
            if (Modifiers.HasFlag(ModKeys.Control)) b.Append("C");
            if (Modifiers.HasFlag(ModKeys.Alt)) b.Append("A");
            if (Modifiers.HasFlag(ModKeys.Shift)) b.Append("S");
            if (b.Length != 0)
                b.Append('-');
            b.Append(Key);
            return b.ToString();
        }
    }
}
#endif