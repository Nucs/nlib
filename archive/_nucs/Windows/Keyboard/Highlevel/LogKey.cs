using System;
using System.Linq;
using System.Windows.Forms;
using nucs.SystemCore.Enums;


namespace nucs.Windows.Keyboard.Highlevel {
    
    public class LogKey {
        private KeysConverter kc_instance { get; set; }
        
        public Keys Key { get; set; }
        public IntPtr KeyboardLayout { get; set; }
        
        private long _handleSerialize {
            get { return KeyboardLayout.ToInt64(); }
            set { KeyboardLayout = new IntPtr(value); }
        }
        
        public ModKeys Modifiers { get; set; }

        /// <summary>
        ///     Is Shift key was held down when this character was hit.
        /// </summary>
        public bool IsShiftPressed => Modifiers.HasFlag(ModKeys.Shift) && (Modifiers.HasFlag(ModKeys.LKey) || Modifiers.HasFlag(ModKeys.RKey));

        /// <summary>
        ///     Is control key was held down when this character was hit.
        /// </summary>
        public bool IsCtrlPressed => Modifiers.HasFlag(ModKeys.Control) && (Modifiers.HasFlag(ModKeys.LKey) || Modifiers.HasFlag(ModKeys.RKey));

        /// <summary>
        ///     Is alt key was held down when this character was hit.
        ///     (Alt is equivalent to Menu key)
        /// </summary>
        public bool IsAltPressed => Modifiers.HasFlag(ModKeys.Alt) && (Modifiers.HasFlag(ModKeys.LKey) || Modifiers.HasFlag(ModKeys.RKey));

        //todo take capslock in count! if (Control.IsKeyLocked()
        public string AsChar {
            get {
                var txt =  KeyboardLanguage.VKCodeToUnicode((uint)Key, KeyboardLayout);
                return Modifiers == ModKeys.RShift || Modifiers == ModKeys.LShift ? txt.ToUpper() : txt.ToLower();
            }
        }

        /// <summary>
        ///     Converts the logkey to a string, shift press to RShift or LShift, Esc to Escape.
        /// </summary>
        public string AsKeyguide {
            get {
                var kc = kc_instance ?? (kc_instance = new KeysConverter());
                var converted = kc.ConvertToString(Key) ?? "";
                if (converted.Length == 1)
                    return AsChar;
                if (converted.CompareOrdinal("OemPeriod") == 0)
                    return ".";
                if (converted.CompareOrdinal("OemQuestion") == 0)
                    return "?";
                if (converted.CompareOrdinal("Oemtilde") == 0)
                    return "`";
                if (converted.CompareOrdinal("Oemtilde") == 0)
                    return "`";
                if (converted.Length==4 && converted.StartsWith("Oem") && char.IsDigit(converted[3]))
                    return converted[3].ToString();
                if (converted.Length == 7 && converted.StartsWith("NumPad") && char.IsDigit(converted[6]))
                    return converted[6].ToString();
                if (converted.CompareOrdinal("Oemcomma") == 0)
                    return ",";
                if (converted.StartsWith("Oem") && converted.Length>=4)
                    converted = converted.Substring(3);
                return converted;

            }
        }
        /// <summary>
        ///     Is it a number or a letter?
        /// </summary>
        private bool IsLetterOrNumber() { //doesnt detect if a letter on other languages... =\ so it wont translate in the AsChar at keyguide
            return Key >= Keys.A && Key <= Keys.Z || Key >= Keys.D0 && Key <= Keys.D9 || Key >= Keys.NumPad0 && Key <= Keys.NumPad9;
        }
        



    }
}