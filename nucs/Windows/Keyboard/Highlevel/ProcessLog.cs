#region

using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nucs.Collections;
using nucs.Windows.Processes;
using ProtoBuf;
using Timer = System.Timers.Timer;

#endregion

namespace nucs.Windows.Keyboard.Highlevel {
    [ProtoContract]
    public class ProcessLog {
        internal CountdownTimer CDTimer { get; private set; }
        public event Action<LoggedLine> LineClosed;

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
            LineClosed?.Invoke(Active);
            Active = null;
        }
    }

    internal class CountdownTimer {
        public event Action Elapsed;

        public bool Working
        {
            get { return t.Enabled; }
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        private DateTime? starttime;

        /*        public int ElapsedMilliseconds => (DateTime.Now- (starttime??DateTime.Now)).Milliseconds;
                public int RemainingMilliseconds => Interval - (DateTime.Now- (starttime??DateTime.Now)).Milliseconds;*/

        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                enabled = value > 0;
                if (value <= 0) {
                    t.Interval = 1;
                    return;
                }

                t.Interval = value;
            }
        }

        internal bool enabled = false;
        private int _interval;
        private readonly Timer t;

        public CountdownTimer(int interval, bool start = false) {
            _interval = interval;
            enabled = interval > 0;
            t = new Timer(interval <= 0 ? 1 : interval) {AutoReset = false, Enabled = false};

            t.Elapsed += (sender, args) => {
                             Stop();
                             _wait_sem.Release(100);
                             Elapsed?.Invoke();
                         };

            if (start) Start();
        }

        public void Start() {
            starttime = DateTime.Now;
            t.Start();
        }

        private readonly SemaphoreSlim _wait_sem = new SemaphoreSlim(0);

        public void Wait() {
            if (t.Enabled)
                _wait_sem.Wait();
        }

        public void Stop() {
            t.Stop();
            starttime = null;
            enabled = false;
        }

        /// <summary>
        /// Resets/restarts
        /// </summary>
        public void Reset() {
            Stop();
            Start();
        }
    }
}