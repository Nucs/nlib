﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using nucs.Threading;
using Nito.AsyncEx.Internal;

// Original idea by Stephen Toub: http://blogs.msdn.com/b/pfxteam/archive/2012/02/11/10266920.aspx

namespace Nito.AsyncEx {
    /// <summary>
    ///     An async-compatible manual-reset event.
    /// </summary>
    [DebuggerDisplay("Id = {Id}, IsSet = {GetStateForDebugger}")]
    [DebuggerTypeProxy(typeof(DebugView))]
    [DebuggerStepThrough]
    public sealed class AsyncManualResetEvent : IWaitable {
        /// <summary>
        ///     The object used for synchronization.
        /// </summary>
        private readonly object _sync;

        /// <summary>
        ///     The semi-unique identifier for this instance. This is 0 if the id has not yet been created.
        /// </summary>
        private int _id;

        /// <summary>
        ///     The current state of the event.
        /// </summary>
        private TaskCompletionSource _tcs;

        /// <summary>
        ///     Creates an async-compatible manual-reset event.
        /// </summary>
        /// <param name="set">Whether the manual-reset event is initially set or unset.</param>
        public AsyncManualResetEvent(bool set) {
            _sync = new object();
            _tcs = new TaskCompletionSource();
            if (set) {
                //Enlightenment.Trace.AsyncManualResetEvent_Set(this, _tcs.Task);
                _tcs.SetResult();
            }
        }
        #if !NET4
        public TaskAwaiter GetAwaiter() {
            return WaitAsync().GetAwaiter();
        }
        #endif
        /// <summary>
        ///     Creates an async-compatible manual-reset event that is initially unset.
        /// </summary>
        public AsyncManualResetEvent()
            : this(false) {}

        [DebuggerNonUserCode]
        private bool GetStateForDebugger {
            get { return _tcs.Task.IsCompleted; }
        }

        /// <summary>
        ///     Gets a semi-unique identifier for this asynchronous manual-reset event.
        /// </summary>
        public int Id {
            get { return IdManager<AsyncManualResetEvent>.GetId(ref _id); }
        }

        /// <summary>
        ///     Whether this event is currently set. This member is seldom used; code using this member has a high possibility of
        ///     race conditions.
        /// </summary>
        public bool IsSet {
            get {
                lock (_sync)
                    return _tcs.Task.IsCompleted;
            }
        }
        #region WaitAsync

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task WaitAsync() {
            lock (_sync) {
                var ret = _tcs.Task;
                //Enlightenment.Trace.AsyncManualResetEvent_Wait(this, ret);
                return ret;
            }
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(int millisecondsTimeout) {
            lock (_sync) {
                if (IsSet)
                    return Tasky.FromResult(true);
                var ret = _tcs.Task;
                //Enlightenment.Trace.AsyncManualResetEvent_Wait(this, ret);
                return Tasky.Run(() => ret.Wait(millisecondsTimeout));
            }
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken) {
            lock (_sync) {
                if (IsSet)
                    return Tasky.FromResult(true);
                var ret = _tcs.Task;
                //Enlightenment.Trace.AsyncManualResetEvent_Wait(this, ret);
                return Tasky.Run(() => ret.Wait(millisecondsTimeout, cancellationToken));
            }
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(TimeSpan timespan) {
            return WaitAsync(Convert.ToInt32(timespan.TotalMilliseconds));
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(TimeSpan timespan, CancellationToken cancellationToken) {
            return WaitAsync(Convert.ToInt32(timespan.TotalMilliseconds), cancellationToken);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task WaitAsync(CancellationToken cancellationToken) {
            lock (_sync) {
                var ret = _tcs.Task;
                //Enlightenment.Trace.AsyncManualResetEvent_Wait(this, ret);
                return Tasky.Run(() => ret.Wait(cancellationToken));
            }
        }
#endregion

        #region Wait
        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public void Wait() {
            WaitAsync().Wait();
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public bool Wait(int milliseconds) {
            return WaitAsync().Wait(milliseconds);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public bool Wait(TimeSpan timespan) {
            return WaitAsync().Wait(timespan);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public bool Wait(int milliseconds, CancellationToken cancellationToken) {
            return WaitAsync().Wait(milliseconds, cancellationToken);
        }
        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        /// <param name="cancellationToken">
        ///     The cancellation token used to cancel the wait. If this token is already canceled, this
        ///     method will first check whether the event is set.
        /// </param>
        public void Wait(CancellationToken cancellationToken) {
            var ret = WaitAsync();
            if (ret.IsCompleted)
                return;
            ret.Wait(cancellationToken);
        }

#endregion

        /// <summary>
        ///     Sets the event, atomically completing every task returned by <see cref="WaitAsync" />. If the event is already set,
        ///     this method does nothing.
        /// </summary>
        public void Set() {
            if (IsSet)
                return;
            lock (_sync) {
                if (IsSet)
                    return;
                _tcs.SetResult();
            }
        }

        /// <summary>
        ///     Resets the event. If the event is already reset, this method does nothing.
        /// </summary>
        public void Reset() {
            lock (_sync) {
                if (_tcs.Task.IsCompleted)
                    _tcs = new TaskCompletionSource();
                //Enlightenment.Trace.AsyncManualResetEvent_Reset(this, _tcs.Task);
            }
        }

        // ReSharper disable UnusedMember.Local
        [DebuggerNonUserCode]
        private sealed class DebugView {
            private readonly AsyncManualResetEvent _mre;

            public DebugView(AsyncManualResetEvent mre) {
                _mre = mre;
            }

            public int Id {
                get { return _mre.Id; }
            }

            public bool IsSet {
                get { return _mre.GetStateForDebugger; }
            }

            public Task CurrentTask {
                get { return _mre._tcs.Task; }
            }
        }

        // ReSharper restore UnusedMember.Local
    }
}