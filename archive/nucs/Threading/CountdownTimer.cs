﻿#region

using System;
using System.Threading;
using Timer = System.Timers.Timer;

#endregion

namespace nucs.Threading {
    public class CountdownTimer {
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