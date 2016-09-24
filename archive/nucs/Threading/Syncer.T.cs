using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace nucs.Threading {
    [DebuggerStepThrough]
    [DebuggerDisplay("IsSet = {IsSet}, Pointer = {_pointer}")]
    public class Syncer<T> : IDisposable, IWaitress<T>, ISyncProvider {
        private readonly AsyncManualResetEvent _reset = new AsyncManualResetEvent();
        private readonly object syncer = new object();
        private T _pointer;

        public bool IsSet => _reset.IsSet;

        public void Dispose() {
            _pointer = default(T); //release reference
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public async Task<T> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken) {
            var result = await _reset.WaitAsync(millisecondsTimeout, cancellationToken);
            if (result == false)
                return default(T);
            lock (syncer) {
                var local = _pointer;
                return local;
            }
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<T> WaitAsync() {
            return WaitAsync(-1, CancellationToken.None);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<T> WaitAsync(int millisecondsTimeout) {
            return WaitAsync(millisecondsTimeout, CancellationToken.None);
        }


        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<T> WaitAsync(TimeSpan timespan) {
            return WaitAsync(Convert.ToInt32(timespan.TotalMilliseconds), CancellationToken.None);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<T> WaitAsync(TimeSpan timespan, CancellationToken cancellationToken) {
            return WaitAsync(Convert.ToInt32(timespan.TotalMilliseconds), cancellationToken);
        }

        /// <summary>
        ///     Asynchronously waits for this event to be set.
        /// </summary>
        public Task<T> WaitAsync(CancellationToken cancellationToken) {
            return WaitAsync(-1, cancellationToken);
        }

        public T Wait() {
            return WaitAsync().Result;
        }

        /// <summary>
        ///     Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a
        ///     signal, while observing a <see cref="T:System.Threading.CancellationToken" />.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.OperationCanceledException">
        ///     <paramref name="cancellationToken" /> was canceled.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        ///     The object has already been disposed or the
        ///     <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has
        ///     been disposed.
        /// </exception>
        public T Wait(CancellationToken cancellationToken) {
            return WaitAsync(cancellationToken).Result;
        }

        public T Wait(int milliseconds, CancellationToken cancellationToken) {
            return WaitAsync(milliseconds, cancellationToken).Result;
        }

        public T Wait(TimeSpan timespan) {
            return WaitAsync(timespan).Result;
        }

        public T Wait(int millisecondsTimeout) {
            return WaitAsync(millisecondsTimeout).Result;
        }

        public void Set(T o) {
            lock (syncer) {
                _pointer = o;
                _reset.Set();
            }
        }

        public void Reset() {
            lock (syncer) {
                _pointer = default(T);
                _reset.Reset();
            }
        }

        public T Take() {
            return Wait(0);
        }

        public override string ToString() {
            return $"State={IsSet}, Item={_pointer}";
        }

        public object Sync => this.syncer;
    }

    public interface ISyncProvider {
        /// <summary>
        /// The sync to sync this entire class
        /// </summary>
        object Sync { get; }
    }
}