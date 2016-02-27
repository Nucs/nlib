using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nucs.SystemCore.Enums;


namespace nucs.Windows.Keyboard.Highlevel {
    
    public class LoggedLine {
        
        public List<LogKey> Content { get; private set; }

        public DateTime? StartRecord { get; set; }
        
        private DateTime _startRecordSerialize {
            get {
                return StartRecord.HasValue == false ? DateTime.MinValue : StartRecord.Value;
            }
            set { StartRecord = value; }
        }

        public DateTime? EndRecord { get; set; }
        
        private DateTime _endRecordSerialize {
            get {
                return StartRecord.HasValue == false ? DateTime.MinValue : StartRecord.Value;
            }
            set { StartRecord = value; }
        }

        public LoggedLine() {
            Content = new List<LogKey>(0);
            StartRecord = DateTime.Now;
        }

        public void AddContent(LogKey key) {
            if (EndRecord != null)
                throw new InvalidOperationException("Can't add content to a closed record.");
            Content.Add(key);
        }

        public void EndRecording() {
            EndRecord = DateTime.Now;
        }

        /// <summary>
        ///     Detailed output to any keys.
        /// </summary>
        public string ToStringKeyGuide() {
            bool ctrl=false, alt=false, shift=false;
            StringBuilder s = new StringBuilder();
            foreach (var k in Content.ToList()) {

                var guidekey = k.AsKeyguide.Replace("Menu","Alt"); //cache it and replace menu with alt.

                //check if req closing
                if (guidekey.Contains("Control")) {
                    if (ctrl==false) { //means just the control was pressed
                        goto _outputkey;
                    }
                    s.Append(":->");
                    ctrl = false;
                    continue;
                }
                //check if req opening
                if ((ctrl == false && k.Modifiers.HasFlag(ModKeys.Control))) {
                    ctrl = true;
                    s.AppendFormat("<{0}Ctrl-:", k.Modifiers.HasFlag(ModKeys.LKey) ? "L" : k.Modifiers.HasFlag(ModKeys.RKey) ? "R" : "");
                }


                if (guidekey.Contains("Alt")) { //test if given as Menu or Alt
                    if (alt==false) { //means just the control was pressed
                        goto _outputkey;
                    }
                    s.Append(":->");
                    alt = false;
                    continue;
                }
                //check if req opening
                if ((alt == false && k.Modifiers.HasFlag(ModKeys.Alt))) {
                    alt = true;
                    s.AppendFormat("<{0}Alt-:", k.Modifiers.HasFlag(ModKeys.LKey) ? "L" : k.Modifiers.HasFlag(ModKeys.RKey) ? "R" : "");
                }


                if (guidekey.Contains("Shift")) {
                    if (shift==false) { //means just the control was pressed
                        goto _outputkey;
                    }
                    s.Append(":->");
                    shift = false;
                    continue;
                }
                //check if req opening
                if ((shift == false && k.Modifiers.HasFlag(ModKeys.Shift))) {
                    shift = true;
                    s.AppendFormat("<{0}Shift-:", k.Modifiers.HasFlag(ModKeys.LKey) ? "L" : k.Modifiers.HasFlag(ModKeys.RKey) ? "R" : "");
                }

            _outputkey:

                if (guidekey.EndsWith("key", true, CultureInfo.InvariantCulture)) guidekey = guidekey.Substring(0, guidekey.Length - 3); //kill ending of LControlKey to LControl
                if (guidekey.Length > 1)
                    s.Append("<" + guidekey + ">");
                else
                    s.Append(IsValidKey(k.Key) ? k.AsChar : guidekey);


                //key itself - if longer than 1 char, escape it.
            }
            return s.ToString();
        }

        /// <summary>
        ///     Raw text input, ignores all the special keys but letters and numbers.
        /// </summary>
        public string ToStringTextInput() {
            StringBuilder s = new StringBuilder();
            //KeysConverter kc = new KeysConverter();
            var a = Content.Where(c => IsValidKey(c.Key)).ToList();
            foreach (var hotkey in a) {
                s.Append(hotkey.AsChar);
            }
            return s.ToString();
        }

        /// <summary>
        ///     Combination between TextInput and KeyGuide
        /// </summary>
        public string ToStringSmart() {
            bool ctrl=false, alt=false, shift=false;
            StringBuilder s = new StringBuilder();
            foreach (var k in Content.ToList()) {

                var guidekey = k.AsKeyguide.Replace("Menu","Alt"); //cache it and replace menu with alt.

                //check if req closing
                if (guidekey.Contains("Control")) {
                    if (ctrl==false) { //means just the control was pressed
                        goto _outputkey;
                    }
                    s.Append(":->");
                    ctrl = false;
                    continue;
                }
                //check if req opening
                if ((ctrl == false && k.Modifiers.HasFlag(ModKeys.Control))) {
                    ctrl = true;
                    s.AppendFormat("<{0}Ctrl-:", k.Modifiers.HasFlag(ModKeys.LKey) ? "L" : k.Modifiers.HasFlag(ModKeys.RKey) ? "R" : "");
                }


                if (guidekey.Contains("Alt")) { //test if given as Menu or Alt
                    if (alt==false) { //means just the control was pressed
                        goto _outputkey;
                    }
                    s.Append(":->");
                    alt = false;
                    continue;
                }
                //check if req opening
                if ((alt == false && k.Modifiers.HasFlag(ModKeys.Alt))) {
                    alt = true;
                    s.AppendFormat("<{0}Alt-:", k.Modifiers.HasFlag(ModKeys.LKey) ? "L" : k.Modifiers.HasFlag(ModKeys.RKey) ? "R" : "");
                }


                if (guidekey.Contains("Shift")) {
                    if (shift==false) { //means just the control was pressed
                        goto _outputkey;
                    }
                    s.Append(":->");
                    shift = false;
                    continue;
                }
                //check if req opening
                if ((shift == false && k.Modifiers.HasFlag(ModKeys.Shift))) {
                    shift = true;
                    s.AppendFormat("<{0}Shift-:", k.Modifiers.HasFlag(ModKeys.LKey) ? "L" : k.Modifiers.HasFlag(ModKeys.RKey) ? "R" : "");
                }

            _outputkey:

                if (guidekey.EndsWith("key", true, CultureInfo.InvariantCulture)) guidekey = guidekey.Substring(0, guidekey.Length - 3); //kill ending of LControlKey to LControl
                //map specials
                switch (guidekey) {
                    case "Space":
                        s.Append(' ');
                        break;
                    case "Oemcomma":
                        s.Append(',');
                        break;
                    case "Enter":
                    case "Return":
                        s.Append("<Enter>" + Environment.NewLine);
                        break;
                    case "Back":
                        if (IsValidKey(k.Key)) {
                            var lc = s[s.Length - 1];
                            s.Remove(s.Length - 1, 1); //delete last
                            s.Append("<B:" + lc + ">");
                        } else {
                            s.Append("<B>");
                        }
                        break;
                    default:
                        if (guidekey.Length > 1)
                            s.Append("<" + guidekey + ">");
                        else
                            s.Append(IsValidKey(k.Key) ? k.AsChar : guidekey);
                        break;
                }

                //key itself - if longer than 1 char, escape it.
            }
            return s.ToString();
        }

        /// <summary>
        ///     Is it a number or a letter?
        /// </summary>
        public static bool IsValidKey(Keys key) {
            return key >= Keys.A && key <= Keys.Z || key >= Keys.D0 && key <= Keys.D9 || key >= Keys.NumPad0 && key <= Keys.NumPad9;
        }


        public override string ToString() {
            return ToStringSmart();
        }
    }
}