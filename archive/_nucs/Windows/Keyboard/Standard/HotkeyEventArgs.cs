#if !AV_SAFE
using System;
using System.Linq;
using System.Windows.Forms;
#if (NET35 || NET3 || NET2)
using nucs.SystemCore.Enums;
#endif

namespace nucs.Windows.Keyboard {

    /// <summary>
    ///     Provides arguments for <see cref="BaseKeyboardManager"/> key events.
    /// </summary>
    public class HotkeyEventArgs {

        /// <summary>
        /// The Hotkey that was trigger during the event.
        /// </summary>
        public Hotkey Hotkey { get; private set; }

        /// <summary>
        /// The key that was pressed.
        /// </summary>
        public Keys Key {
            get { return Hotkey.Key; }
        }

        /// <summary>
        /// The modifiers that has been pressed.
        /// </summary>
        public ModKeys Modifiers {
            get { return Hotkey.Modifiers; }
        }

        /// <summary>
        /// Exports the side from <see cref="Modifiers"/>.
        /// </summary>
        public ModKeys ModifierSide {
            get { return Modifiers.ExportSide(); }
        }

        /// <summary>
        /// Check in <see cref="Modifiers"/> if control pressed
        /// </summary>
        public bool Control {
            get {
                return Modifiers.HasFlag(ModKeys.Control);
            }
        }

        /// <summary>
        /// Check in <see cref="Modifiers"/> if alt pressed
        /// </summary>
        public bool Alt {
            get {
                return Modifiers.HasFlag(ModKeys.Alt);
            }
        }

        /// <summary>
        /// Check in <see cref="Modifiers"/> if shfit pressed
        /// </summary>
        public bool Shift {
            get {
                return Modifiers.HasFlag(ModKeys.Shift);
            }
        }

        /// <summary>
        ///     (HAS BEEN SET TO INTERNAL BECAUSE INTEGRATION BECAME PROBLEMATIC) 
        ///     Should the keyevent cancel? a little bit buggy with the entire system. recommanded to avoid using it.
        /// </summary>
        internal bool Handled { get; set; }

        /// <summary>
        /// Description of the hotkey press
        /// </summary>
        public string Description {
            get { return Hotkey.Description; }
        }

        /// <summary>
        /// Turns the key to Virtual Key code.
        /// </summary>
        public uint VKey {
            get { return Hotkey.VKey; }
        }

        private HotkeyEventArgs() { }

        public static HotkeyEventArgs Create(Hotkey hk) {
            if (hk == null) return null;
            var args = new HotkeyEventArgs {Hotkey = hk};
            return args;
        }

        /// <summary>
        /// Supports event from AKey press.
        /// </summary>
        /// <param name="description">Description of the specific key, must be unique.</param>
        public static HotkeyEventArgs Create(AKey k, string description) {
            return new HotkeyEventArgs {Hotkey = Hotkey.Create(k, description)};
        }

        public override string ToString() { return (Hotkey != null ? (Description!=null ? Description + ": " : "") + Hotkey : ""); }
    }
}
#endif