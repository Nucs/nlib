using System.Threading;

namespace nucs.Threading {

    /// <summary>
    /// A class that allows cheap and comfortable way to wait for anything that can be expressed with a boolean to be tested and may take async time
    /// </summary>
    public class TimeLimitedInspector {
        /// <summary>
        /// A delegate of a method that provides if the task was successful or not for <see cref="TimeLimitedInspector"/>
        /// </summary>
        /// <param name="timeleft">The time left before the inspection expires</param>
        /// <returns>Was successful or not</returns>
        public delegate bool IfTrueFinishHandler(TimeLimitedInspector inspector, int timeleft);
        /// <summary>
        /// Time for the inspection to happen. if the time expires then it will set WasSuccessful to false and HasFinished to True
        /// </summary>
        public int TimeLimit { get; private set; }
        /// <summary>
        /// The inspecting method
        /// </summary>
        public IfTrueFinishHandler Action { get; private set; }
        /// <summary>
        /// Has the inspection finished
        /// </summary>
        public bool HasFinished { get; private set; }
        /// <summary>
        /// Was the inspection succesful
        /// </summary>
        public bool WasSuccessful { get; private set; }
        /// <summary>
        /// Was the inspection aborted
        /// </summary>
        public bool WasAborted { get; private set; }
        /// <summary>
        /// The core thread for the inspection
        /// </summary>
        internal Thread thread;
        /// <summary>
        /// used to hold the thread that asked for Wait();
        /// </summary>
        private readonly ManualResetEventSlim holder = new ManualResetEventSlim(false);
        /// <summary>
        /// A class that allows cheap and comfortable way to wait for anything that can be expressed with a boolean to be tested and may take async time
        /// </summary>
        /// <param name="milliseconds">The maximum time to allow the inspection to happen</param>
        /// <param name="action">The inspecting method</param>
        public TimeLimitedInspector(int milliseconds, IfTrueFinishHandler action) {
            TimeLimit = milliseconds;
            Action = action;
        }
        /// <summary>
        /// Insures that this instance can run once unless Reset() is called
        /// </summary>
        private bool _singleshot = false;
        /// <summary>
        /// Starts the inspection
        /// </summary>
        public void Start() {
            if (_singleshot) return;
            _singleshot = true;
            thread = new Thread(() => {
                                    int time_left = TimeLimit;
                                    int time_differentiate = time_left/100;
                                    do {
                                        if (Action(this, time_left) == true) {
                                            WasSuccessful = true;
                                            HasFinished = true;
                                            goto _done;
                                        }
                                        Thread.Sleep(time_differentiate);
                                    }  while ((time_left -= time_differentiate) > 0);  
                            _failed:
                                    HasFinished = true;
                                    WasSuccessful = false;
                              _done:
                                    holder.Set();
            });
            thread.Start();
        }
        /// <summary>
        /// Starts the inspection and waits for it to end and then returns 'WasSuccessful'
        /// </summary>
        /// <returns></returns>
        public bool StartAndWait() {
            if (_singleshot)
                return WasSuccessful;
            Start();
            holder.Wait();
            return WasSuccessful;
        }

        /// <summary>
        /// Holds the calling thread untill inspection ends
        /// </summary>
        /// <returns></returns>
        public bool Wait() {
            holder.Wait();
            return WasSuccessful;
        }
/*        /// <summary>
        /// Resets the entire instance and aborts incase the inspection is on.
        /// </summary>
        public void Reset() {
            
            HasFinished = false;
            WasAborted = false;
            this.WasSuccessful = false;
            this._singleshot = false;
            holder.Reset();
        }*/

        /// <summary>
        /// Aborts immediatly the inspection.
        /// </summary>
        public void Abort() {
            if (_singleshot && HasFinished) return;
            HasFinished = true;
            WasSuccessful = false;
            WasAborted = true;
            holder.Set();
        }

    }
}