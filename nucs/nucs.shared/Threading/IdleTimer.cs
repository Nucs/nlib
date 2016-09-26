using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace STLMiner {
    /// <summary>
    /// A timer for monitoring user's being idle or active based on a timeout given in the constructor.
    /// This object starts working from the construction and cannot be stopped but restarting or completely disposing (disabling).
    /// </summary>
    internal class IdleTimer : IDisposable {
        #region Properties and Constructor

        /// <summary>
        /// User has entered Idle state (AFK) after <see cref="Timeout"/> milliseconds.
        /// </summary>
        public event Action Idled;
        /// <summary>
        /// User has exited Idle state to active.
        /// </summary>
        public event Action Returned;

        private LASTINPUTINFO lastInputInf;
        private int _frequency;
        private int _timeout;
        private CancellationTokenSource _sampler_canceller;
        private UserActivityState _state = UserActivityState.Unknown;
        private bool _hasStarted = false;
        /// <summary>
        /// The frequency at which it will count how much time the user is idle (milliseconds).
        /// The closer to 0, the more accurate the result will be but more cpu will be used.
        /// </summary>
        public int Frequency {
            get { return _frequency; }
            set { _frequency = value; }
        }

        public bool HasStarted {
            get { return _hasStarted; }
        }

        /// <summary>
        /// Represents the timeout time at which, how much time it takes to declare the user as idle and invoke IdleTimeout event (milliseconds).
        /// </summary>
        public int Timeout {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// The user's state at the current moment.
        /// </summary>
        public UserActivityState State {
            get { return _state; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">Represents the timeout time at which, how much time it takes to declare the user as idle and invoke IdleTimeout event (milliseconds)</param>
        /// <param name="frequency">The frequency at which it will count how much time the user is idle.
        /// The closer to 0, the more accurate the result will be but more cpu will be used. (milliseconds)
        /// I would recommand a 1/10 rate for frequency. 250 freq for 25sec timeout</param>
        /// <param name="autoStart">Should the idleTimer start?</param>
        public IdleTimer(int timeout = 25000, int frequency = 250, bool autoStart = true) {
            _timeout = timeout;
            _frequency = frequency;
            if (autoStart) 
                _start();
        }

        #endregion

        #region Methods
        
        #region Public

        /// <summary>
        /// Start the idlerTimer. Once started, cannot be stopped unless you dispose it.
        /// </summary>
        public void Start() {
            _start();
        }

        public void Reset() {
            _sampler_canceller.Cancel();
            _state = UserActivityState.Unknown;
            _start();
        }

        public void Dispose() {
            _sampler_canceller.Cancel();
            _sampler_canceller.Dispose();
            Idled = null;
            Returned = null;
        }

        #endregion

        #region Private

        private void _start() {
            if (_hasStarted)
                return;
            _hasStarted = true;
            Tasky.Run(() => _sampler((_sampler_canceller = new CancellationTokenSource()).Token));
            lastInputInf = new LASTINPUTINFO();
        }

        private void _sampler(CancellationToken c) {
            var was_called = false;
            _state = UserActivityState.Active;
            while (true) {
                if (c.IsCancellationRequested) 
                    break;
                Thread.Sleep(_frequency);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (c.IsCancellationRequested) //might change after a delay.
                    break;
                if (GetLastInputTime() >= _timeout) { //timedout. 1533 >= 500
                    if (was_called) continue; //has already called b4?
                    was_called = true; //was -> set called now.
                    _state = UserActivityState.Idle;
                    if (Idled != null) //call
                        Idled();
                } else { //didn't timeout, set not called.
                    _state = UserActivityState.Active;
                    if (was_called) { //moved from idle to active
                        was_called = false;
                        if (Returned != null)
                            Returned();
                    }
                }
            }
        }


        [DllImport("user32.dll")] private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        
        /// <summary>
        /// Gets the idle time of the user (keyboard-mouse input) in millisecs
        /// </summary>
        private int GetLastInputTime() {
            int idletime = 0;
            lastInputInf.cbSize = Marshal.SizeOf(lastInputInf);
            lastInputInf.dwTime = 0;

            if (GetLastInputInfo(ref lastInputInf)) {
                idletime = Environment.TickCount - lastInputInf.dwTime;
            }

            if (idletime != 0) {
                return idletime;
            }
            return 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO {
            [MarshalAs(UnmanagedType.U4)] public int cbSize;
            [MarshalAs(UnmanagedType.U4)] public int dwTime;
        }

        #endregion

        #endregion
    }

    public enum UserActivityState {
        Idle,
        Active,
        Unknown
    }
}