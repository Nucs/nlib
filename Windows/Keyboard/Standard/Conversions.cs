using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace nucs.Windows.Keyboard {
    public static class KeyTypeEnumConversions {
        public static Keys ToKeys(this Key key) {
            return (Keys)KeyInterop.VirtualKeyFromKey(key);
        }

        public static Key ToKey(this Keys keys) {
            return KeyInterop.KeyFromVirtualKey((int)keys);
        }

        public static string ToUnicode(this Keys key) {
            return KeyboardLanguage.VKCodeToUnicode((uint) key);
        }
    }
}
