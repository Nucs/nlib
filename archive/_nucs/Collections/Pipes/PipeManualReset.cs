using System;
using System.Threading;

namespace nucs.Collections.Concurrent.Pipes {
    public class PipeManualReset<T> : Pipe<T>, IDisposable {
        private readonly ManualResetEventSlim _reset = new ManualResetEventSlim(false);
        public PipeManualReset() {}

        public PipeManualReset(IPipe<T> receiveFrom) {
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
        public override void IncomingItem(T o) {
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

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set.</summary>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public void Wait() {
            _reset.Wait();
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> receives a signal, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.OperationCanceledException">
        /// <paramref name="cancellationToken" /> was canceled.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
        public void Wait(CancellationToken cancellationToken) {
            _reset.Wait(cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a <see cref="T:System.TimeSpan" /> to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out. -or-The number of milliseconds in <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />. </exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public bool Wait(TimeSpan timeout) {
            return _reset.Wait(timeout);
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
        public bool Wait(TimeSpan timeout, CancellationToken cancellationToken) {
            return _reset.Wait(timeout, cancellationToken);
        }

        /// <summary>Blocks the current thread until the current <see cref="T:System.Threading.ManualResetEventSlim" /> is set, using a 32-bit signed integer to measure the time interval.</summary>
        /// <returns>true if the <see cref="T:System.Threading.ManualResetEventSlim" /> was set; otherwise, false.</returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
        /// <exception cref="T:System.InvalidOperationException">The maximum number of waiters has been exceeded.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The object has already been disposed.</exception>
        public bool Wait(int millisecondsTimeout) {
            return _reset.Wait(millisecondsTimeout);
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
        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) {
            return _reset.Wait(millisecondsTimeout, cancellationToken);
        }

        /// <summary>Gets whether the event is set.</summary>
        /// <returns>true if the event has is set; otherwise, false.</returns>
        public bool IsSet {
            get { return _reset.IsSet; }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            _reset.Dispose();
        }
        #endregion

    }

}
