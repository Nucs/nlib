using System;
using System.Linq;
using System.Windows.Forms;
using nucs.Collections;
using nucs.Threading;
using nucs.Windows.Processes;
using ProtoBuf;

namespace nucs.Windows.Keyboard.Highlevel {
    [ProtoContract]
    public class ProcessLog {
        public CountdownTimer CDTimer { get; private set; }

        [ProtoMember(1)]
        public ProcessInfo Process { get; set; }

        [ProtoMember(2)]
        public ImprovedList<LoggedLine> Logs { get; set; }
        
        public ProcessLog() {
            Logs = new ImprovedList<LoggedLine>(0);
            CDTimer = new CountdownTimer(5000);
            CDTimer.Elapsed += CdTimerOnElapsed;
        }

        private void CdTimerOnElapsed() {
            if (Control.ModifierKeys == Keys.None) {
                //Console.WriteLine("[CDTimer] Row Closed");
                CloseActiveLine();
            } else {
                CDTimer.Reset();
                //Console.WriteLine("[CDTimer] Timer Restarted, ctrl alt shift is held down.");
            }
        }

        public void AddContent(LogKey key) {
            var al = GetActiveLine();
            al.AddContent(key);
            CDTimer.Reset();
        }

        /// <summary>
        ///     Concenates all lines and splits them with newline.
        /// </summary>
        /// <returns></returns>
        public string GetInputString() {
            var r = "@_" + Process.Name + Environment.NewLine;
            r += Logs.Select(log => log.ToStringTextInput()).StringJoin(Environment.NewLine);
            return r;
        }

        public string GetKeyguideString() {
            var r = "@_" + Process.Name + Environment.NewLine;
            r += Logs.Select(log =>
                (log.StartRecord.Value.ToString("HH:mm:ss")
                 + "-"
                 + (log.EndRecord ?? DateTime.Now).ToString("HH:mm:ss")
                 + " - "
                 + log.ToStringKeyGuide())).StringJoin(Environment.NewLine) + Environment.NewLine;
            return r;
        }

        private LoggedLine Active = null;
        private LoggedLine GetActiveLine() {
            if (Active == null) {
                Active = new LoggedLine();
                Logs.Add(Active);
            }
            return Active;
        }

        private void CloseActiveLine() {
            if (Active == null) return;
            Active.EndRecording();
            Active = null;
        }
    }
}
