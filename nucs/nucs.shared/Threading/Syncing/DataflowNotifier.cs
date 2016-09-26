using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using nucs.Collections;
using nucs.Collections.Concurrent.Pipes;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace nucs.Threading.Syncers {

    /// <summary>
    ///     Collects all objects into a queue and a collection giving other threads an option to wait for an object to arrive
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataflowNotifier<T> : IWaitress<ResultWaiting<T>>, IEnumerable<T> {

        /// <summary>
        ///     Filter that when it returns false, the object wont be queued.
        /// </summary>
        public Func<T, bool> Filter { get; }

        /// <summary>
        ///     Should exception be thrown when Set is called after SetEnd has been called - otherwise it will not 
        /// </summary>
        public bool ThrowOnSetWhenEnded { get; set; } = false;

        /// <summary>
        ///     A side collection that will collect all queued objects into it - works only if Collect is set to true.
        /// </summary>
        public readonly ConcurrentList<T> Collection = new ConcurrentList<T>();
        private readonly PipedConcurrentQueue<T> queue;
        private readonly Syncing.PipeAsyncManualReset<T> awaker;
        private readonly AsyncManualResetEvent endwaiter = new AsyncManualResetEvent(false);
        private readonly object syncer = new object();
        /// <summary>
        ///     Has SetEnded been called.
        /// </summary>
        public bool HasEnded { get; private set; } = false;

        public DataflowNotifier() {
            queue = new PipedConcurrentQueue<T>(PipedQueueStyle.OnEnqueue);
            awaker = new Syncing.PipeAsyncManualReset<T>(queue);
        }

        /// <param name="filter">Apply filter that when false is returned, the item wont be queued.</param>
        public DataflowNotifier(Func<T, bool> filter) : this() {
            Filter = filter;
        }
        /// <summary>
        ///     Set with an object.
        /// </summary>
        /// <param name="obj"></param>
        public void Set(T obj) {
            if (HasEnded) {
                if (ThrowOnSetWhenEnded)
                    throw new InvalidOperationException("Cannot call set after the object has ended");
                return;
            }
            if (Filter != null && Filter(obj) == false) //if was filtered
                return; 
            Collection.Add(obj);
            queue.Enqueue(obj);
        }

        /// <summary>
        ///     Set the End trigger to force Set not to add new items.
        /// </summary>
        public void SetEnd() {
            HasEnded = true;
            endwaiter.Set();
        }
        /// <summary>
        ///     Set the End trigger to force Set not to add new items while adding this item as the last one.
        /// </summary>
        public void SetEnd(T obj) {
            Set(obj);
            SetEnd();
        }

        //internal use only
        internal void Reset() {
            awaker.Reset();
        }
#if !NET4
        public TaskAwaiter<ResultWaiting<T>> GetAwaiter() {
            return WaitAsync().GetAwaiter();
        }
#endif
        #region Return Waits
        #if NET4
        public Task<ResultWaiting<T>> WaitAsync(int milliseconds, CancellationToken cancellationToken) {
            return Tasky.Run(() => {
                _repeat:
                var successful = awaker.WaitAsync(milliseconds, cancellationToken).Result;
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
            });
        }
        #else
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
#endif

        #endregion

        #region Waits
        #region WaitAsync
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
        #endregion
        #region Wait
        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public ResultWaiting<T> Wait() {
            return WaitAsync().Result;
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public ResultWaiting<T> Wait(int milliseconds) {
            return WaitAsync(milliseconds).Result;
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        public ResultWaiting<T> Wait(TimeSpan timespan) {
            return WaitAsync(timespan).Result;
        }

        /// <summary>
        ///     Synchronously waits for this event to be set. This method may block the calling thread.
        /// </summary>
        /// <param name="cancellationToken">
        ///     The cancellation token used to cancel the wait. If this token is already canceled, this
        ///     method will first check whether the event is set.
        /// </param>
        public ResultWaiting<T> Wait(CancellationToken cancellationToken) {
            return WaitAsync(cancellationToken).Result;
        }
        #endregion
        #region WaitEnd
        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set.</summary>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public void WaitEnd() {
            endwaiter.Wait();
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a signal, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public void WaitEnd(CancellationToken cancellationToken) {
            endwaiter.Wait(cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public bool WaitEnd(TimeSpan timeout) {
            return endwaiter.Wait(timeout);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public bool WaitEnd(TimeSpan timeout, CancellationToken cancellationToken) {
            return endwaiter.Wait(Convert.ToInt32(timeout.TotalMilliseconds), cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public bool WaitEnd(int millisecondsTimeout) {
            return endwaiter.Wait(millisecondsTimeout);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public bool WaitEnd(int millisecondsTimeout, CancellationToken cancellationToken) {
            return endwaiter.Wait(millisecondsTimeout, cancellationToken);
        }
        #endregion
        #region WaitAll
#if !NET4
        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set.</summary>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public async Task<T[]> WaitAll() {
            return await WaitAll(-1, CancellationToken.None);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a signal, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public async Task<T[]> WaitAll(CancellationToken cancellationToken) {
            return await WaitAll(-1, cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public async Task<T[]> WaitAll(TimeSpan timeout) {
            return await WaitAll(Convert.ToInt32(timeout.TotalMilliseconds), CancellationToken.None);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public async Task<T[]> WaitAll(TimeSpan timeout, CancellationToken cancellationToken) {
            return await WaitAll(Convert.ToInt32(timeout.TotalMilliseconds), cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public async Task<T[]> WaitAll(int millisecondsTimeout) {
            return await WaitAll(millisecondsTimeout, CancellationToken.None);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public async Task<T[]> WaitAll(int millisecondsTimeout, CancellationToken cancellationToken) {
            var b = await endwaiter.WaitAsync(millisecondsTimeout, cancellationToken);
            if (b == false)
                return null;
            return Collection.ToArray();
        }
        #else
        
        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set.</summary>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public Task<T[]> WaitAll() {
            return WaitAll(-1, CancellationToken.None);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a signal, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public Task<T[]> WaitAll(CancellationToken cancellationToken) {
            return WaitAll(-1, cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public Task<T[]> WaitAll(TimeSpan timeout) {
            return WaitAll(Convert.ToInt32(timeout.TotalMilliseconds), CancellationToken.None);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public Task<T[]> WaitAll(TimeSpan timeout, CancellationToken cancellationToken) {
            return WaitAll(Convert.ToInt32(timeout.TotalMilliseconds), cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public Task<T[]> WaitAll(int millisecondsTimeout) {
            return WaitAll(millisecondsTimeout, CancellationToken.None);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public Task<T[]> WaitAll(int millisecondsTimeout, CancellationToken cancellationToken) {
            return endwaiter.WaitAsync(millisecondsTimeout, cancellationToken).ContinueWith(task => {
                var b = task.Result;
                if (b == false)
                    return null;
                return Collection.ToArray();
            });
        }
        #endif
        #endregion
        #endregion

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>) Collection.ToArray()).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    [DebuggerDisplay("Successful = {Successful}, Result = {Result}")]
    public class ResultWaiting<T> {
        /// <summary>
        ///     If true, item has been received and finished waiting. If false - waiting has expired and no item has been collected.
        /// </summary>
        public bool Successful { get; set; }
        /// <summary>
        /// The item that was set.
        /// </summary>
        public T Result { get; set; }

    }

    public static class DataflowNotifierExtensions {
        /// <summary>
        ///     Once the task finished, it will insert the result into the dataflow.
        /// </summary>
        public static Task<T> InsertIntoDataflow<T>(this Task<T> task, DataflowNotifier<T> flower) {
            return task.ContinueWith(t => {
                flower.Set(t.Result);
                return t.Result;
            });
        }
    }

}


