using System.Collections.Generic;
using System.Linq;

namespace nucs.Windows.Keyboard {
    public static class KeySender {
        public static void SendText(string text) {
            InputSimulator.SimulateTextEntry(text);
        }

        public static void PressKey(KeyCode key) {
            InputSimulator.SimulateKeyPress((VirtualKeyCode)((ushort)(key)));
        }

        public static void PressKey(IEnumerable<KeyCode> modifiers, KeyCode key) {
            InputSimulator.SimulateModifiedKeyStroke(modifiers.Select(j => (VirtualKeyCode)((ushort)(j))), (VirtualKeyCode)((ushort)(key)));
        }

        public static void PressKey(KeyCode modifier, KeyCode key) {
            InputSimulator.SimulateModifiedKeyStroke((VirtualKeyCode)((ushort)(modifier)), (VirtualKeyCode)((ushort)(key)));
        }

        public static void PressKeys(IEnumerable<KeyCode> modifiers, IEnumerable<KeyCode> keys) {
            InputSimulator.SimulateModifiedKeyStroke(modifiers.Select(j => (VirtualKeyCode)((ushort)(j))), keys.Select(j => (VirtualKeyCode)((ushort)(j))));
        }

        public static void PressKeys(KeyCode modifier, IEnumerable<KeyCode> keys) {
            InputSimulator.SimulateModifiedKeyStroke((VirtualKeyCode)((ushort)(modifier)), keys.Select(j => (VirtualKeyCode)((ushort)(j))));
        }
    }
}