using System.Collections.Generic;
using System.Linq;

namespace nucs.Windows.Keyboard {
    public static class KeySender {
        public static void SendText(string text) {
            KeyCodeUtils.SimulateTextEntry(text);
        }

        public static void PressKey(KeyCode key) {
            KeyCodeUtils.SimulateKeyPress(key);
        }

        public static void PressKey(IEnumerable<KeyCode> modifiers, KeyCode key) {
            KeyCodeUtils.SimulateModifiedKeyStroke(modifiers.Select(j => j), key);
        }

        public static void PressKey(KeyCode modifier, KeyCode key) {
            KeyCodeUtils.SimulateModifiedKeyStroke(modifier, key);
        }

        public static void PressKeys(IEnumerable<KeyCode> modifiers, IEnumerable<KeyCode> keys) {
            KeyCodeUtils.SimulateModifiedKeyStroke(modifiers.Select(j => j), keys.Select(j => j));
        }

        public static void PressKeys(KeyCode modifier, IEnumerable<KeyCode> keys) {
            KeyCodeUtils.SimulateModifiedKeyStroke(modifier, keys.Select(j => j));
        }
    }
}