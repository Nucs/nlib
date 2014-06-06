using System;
using System.Linq;
using System.Windows.Forms;

namespace nucs.Windows.Keyboard {

    public class HotkeyEventArgs {
        public BaseKeyboardManager sender { get; private set; }
        public Hotkey Hotkey { get; private set; }

        public Keys Key {
            get { return Hotkey.Key; }
        }

        public Keys Modifiers {
            get { return Hotkey.Modifiers; }
        }

        public string description {
            get { return Hotkey.Description; }
        }

        public uint VKey {
            get { return Hotkey.VKey; }
        }

        private HotkeyEventArgs() { }

        public static HotkeyEventArgs Create(BaseKeyboardManager sender, IntPtr lParam) {
            var param = (uint) lParam.ToInt64();
            var key = (Keys) ((param & 0xffff0000) >> 16);
            var modifiers = (Keys) (param & 0x0000ffff);
            return Create(sender, key, modifiers);
        }

        public static HotkeyEventArgs Create(BaseKeyboardManager sender, Keys key, Keys modifiers) {
            var hke = new HotkeyEventArgs() {sender = sender};
            hke.Hotkey = sender.Registers.FirstOrDefault(hk => hk.Key == key && hk.Modifiers == modifiers);
            return hke.Hotkey == null ? null : hke;
        }
    }
}