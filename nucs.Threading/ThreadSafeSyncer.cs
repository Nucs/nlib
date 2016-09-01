using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace nlib.Threading.FastThreadPool {
    [DebuggerStepThrough]
    public class ThreadSafeSyncer<T> : IDisposable {
        private readonly ManualResetEventSlim _reset = new ManualResetEventSlim();
        private T obj;
        private readonly object syncer = new object();
        private readonly object waiting_syncer = new object();
        public void Set(T o) {
            lock (waiting_syncer) {
                if (_reset.IsSet) {
                    _reset.Wait();
                }
            
                lock (syncer) {
                    obj = o;
                    _reset.Set();
                }
            }
        }

        public void Reset() {
            lock (syncer) {
                obj = default(T);
                _reset.Reset();
            }
        }

        public T Wait() { //todo this wait act
            _reset.Wait();
            return obj;
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a signal, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public T Wait(CancellationToken cancellationToken) {
            _reset.Wait(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            return obj;
        }

        public T Wait(TimeSpan timeout) {
            if (!_reset.Wait(timeout))
                return default(T);
            return obj;
        }

        public T Wait(int millisecondsTimeout) {
            if (!_reset.Wait(millisecondsTimeout))
                return default(T);
            return obj;
        }
        
        public void Dispose() {
            _reset.Dispose();
            obj = default(T); //release reference
        }

        public T Take() {
            return Wait(0);
        }

        public bool IsSet => _reset.IsSet;

        public override string ToString() {
            return $"State={IsSet}, Item={obj}";
        }
    }
}