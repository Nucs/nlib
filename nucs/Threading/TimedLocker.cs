using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace nucs.Threading {
    /// <summary>
    /// Insures <para>Delay</para> in milliseconds between every Wait call, no matter what, that time will pass before it will release the lock from Wait
    /// </summary>
    public class TimedLocker {

        public int Delay {
            get { return _delay; }
            set { _delay = value; counter.Interval = _delay; }
        }
        private int _delay;
        public readonly object LockObject;
        private readonly ManualResetEventSlim reseter = new ManualResetEventSlim(true);
        private readonly CountdownTimer counter;

        public bool Waiting { get { return counter.Working; } }
        /// <summary>
        /// Uses ResetEvent to limit a process once every 'delay' milliseconds after the last time.
        /// </summary>
        /// <param name="lockObj">the lock object of the wait closure</param>
        /// <param name="delay">delay in milliseconds after the last wait</param>
        public TimedLocker(int delay, object lockObj = null) {
            _delay = delay;
            LockObject = lockObj ?? new object();
            counter = new CountdownTimer(delay);
            counter.Elapsed += () => reseter.Set();
        }

        /// <summary>
        /// Waits for a single loop of 'Delay', if it has already passed since last wait call, it will return immediatly.
        /// </summary>
        public void Wait() {
            if (counter.Enabled == false)
                return;
            if (Delay <= 0) {
                if (reseter.IsSet == false) {
                    reseter.Set();
                    counter.Stop();
                }
                return;
            }
                
            lock (LockObject) {
                if (reseter.IsSet) {
                    reseter.Reset();
                    counter.Start();
                    return;
                }
                reseter.Wait();
                reseter.Reset();
                counter.Start();
            }
        }
        /// <summary>
        /// Forces the locker to immediatly finish the waiting loop
        /// </summary>
        public void Reset() {
            reseter.Reset();
        }


        

    }
}
