using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using nucs.Collections.Concurrent.Pipes;
using nucs.Threading.Syncers.Pipes;

namespace nucs.Threading.Syncers {
    public class DataflowNotifier<T> {
        private readonly PipedConcurrentQueue<T> queue;
        private PipeAsyncManualReset<T> awaker;
        private readonly object syncer = new object();
    
        public DataflowNotifier() {
            queue = new PipedConcurrentQueue<T>(PipedQueueStyle.OnEnqueue);
            awaker = new PipeAsyncManualReset<T>(queue);
        }

        public void Set(T obj) {
            queue.Enqueue(obj);
        }

        internal void Reset() {
            awaker.Reset();
        }

        #region Return Waits

        public async Task<ResultWaiting<T>> WaitAsync(int milliseconds, CancellationToken cancellationToken) {
            _repeat:
            var successful = await awaker.WaitAsync(milliseconds, cancellationToken);
            lock (syncer) {
                if (!successful)
                    return new ResultWaiting<T>() {Successful = false};
                T @out;
                if (!queue.Dequeue(out @out))
                    goto _repeat;
                var ret = new ResultWaiting<T>() {Successful = true, Result = @out};
                if (queue.IsEmpty)
                    Reset();

                return ret;
            }
        }

        #endregion

        #region Waits

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<ResultWaiting<T>> WaitAsync() {
            return WaitAsync(-1, CancellationToken.None).ContinueWith(task => {
                if (!task.IsCompleted)
                    return new ResultWaiting<T>() { Successful = false };
                return task.Result;
            });
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<ResultWaiting<T>> WaitAsync(int millisecondsTimeout) {
            return WaitAsync(millisecondsTimeout, CancellationToken.None);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<ResultWaiting<T>> WaitAsync(TimeSpan timespan) {
            return WaitAsync(Convert.ToInt32(timespan.TotalMilliseconds), CancellationToken.None);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<ResultWaiting<T>> WaitAsync(TimeSpan timespan, CancellationToken cancellationToken) {
            return WaitAsync(Convert.ToInt32(timespan.TotalMilliseconds), cancellationToken);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<ResultWaiting<T>> WaitAsync(CancellationToken cancellationToken) {
            return WaitAsync(-1, cancellationToken);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public void Wait() {
            awaker.Wait();
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public bool Wait(int milliseconds) {
            return awaker.Wait(milliseconds);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public bool Wait(TimeSpan timespan) {
            return awaker.Wait(timespan);
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        /// <param name="cancellationToken">
        ///     The cancellation token used to cancel the wait. If this token is already canceled, this
        ///     method will first check whether the event is set.
        /// </param>
        public void Wait(CancellationToken cancellationToken) {
            awaker.Wait(cancellationToken);
        }

        #endregion

    }

    [DebuggerDisplay("Successful = {Successful}, Result = {Result}")]
    public class ResultWaiting<T> {
        public bool Successful { get; set; }
        public T Result { get; set; }
    }
}