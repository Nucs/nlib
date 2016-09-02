using System;
using System.Threading;
using System.Threading.Tasks;
using nucs.Collections.Concurrent.Pipes;
using Nito.AsyncEx;

namespace nucs.Threading.Syncers.Pipes {
    public class PipeAsyncManualReset<T> : Pipe<T> {
        private readonly AsyncManualResetEvent _reset = new AsyncManualResetEvent(false);
        public PipeAsyncManualReset() {}

        public PipeAsyncManualReset(IPipe<T> receiveFrom) {
            this.ConnectFrom(receiveFrom);
        }

        /// <summary>
        /// This event is called when an object comes in.
        /// </summary>
        public event Action<T> Incoming;

        /// <summary>
        ///     When item comes in, call this method! - Required!
        /// </summary>
        /// <param name="o"></param>
        protected override void IncomingItem(T o) {
            base.IncomingItem(o);
            _reset.Set();
            Incoming?.Invoke(o);
        }

        

        #region Implement
        /// <summary>Sets the state of the event to nonsignaled, which causes threads to block.</summary>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public void Reset() {
            _reset.Reset();
        }

        /// <summary>Gets whether the event is set.</summary>
        /// <returns>true if the event has is set; otherwise, false.</returns>
        public bool IsSet => _reset.IsSet;
        
        #region Waits

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task WaitAsync() {
            return _reset.WaitAsync();
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(int millisecondsTimeout) {
            return _reset.WaitAsync(millisecondsTimeout);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken) {
            return _reset.WaitAsync(millisecondsTimeout, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(TimeSpan timespan) {
            return _reset.WaitAsync(timespan);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<bool> WaitAsync(TimeSpan timespan, CancellationToken cancellationToken) {
            return _reset.WaitAsync(timespan, cancellationToken);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task WaitAsync(CancellationToken cancellationToken) {
            return _reset.WaitAsync(cancellationToken);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public void Wait() {
            _reset.Wait();
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public bool Wait(int milliseconds) {
            return _reset.Wait(milliseconds);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public bool Wait(TimeSpan timespan) {
            return _reset.Wait(timespan);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        /// <param name="cancellationToken">
        ///     The cancellation token used to cancel the wait. If this token is already canceled, this
        ///     method will first check whether the event is set.
        /// </param>
        public void Wait(CancellationToken cancellationToken) {
            _reset.Wait(cancellationToken);
        }

        #endregion

        #endregion

    }

}
