using System;
using System.Threading;

namespace nucs.Threading {
    public class Syncer : IDisposable {
        private readonly ManualResetEventSlim _reset = new ManualResetEventSlim();
        public void Set() {
            _reset.Set();
        }

        public void Reset() {
            _reset.Reset();
        }

        public void Wait() {
            _reset.Wait();
        }

        public void Wait(CancellationToken cancellationToken) {
            _reset.Wait(cancellationToken);
        }

        public bool Wait(TimeSpan timeout) {
            return _reset.Wait(timeout);
        }

        public bool Wait(TimeSpan timeout, CancellationToken cancellationToken) {
            return _reset.Wait(timeout, cancellationToken);
        }

        public bool Wait(int millisecondsTimeout) {
            return _reset.Wait(millisecondsTimeout);
        }

        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) {
            return _reset.Wait(millisecondsTimeout, cancellationToken);
        }

        public void Dispose() {
            _reset.Dispose();
        }


        public bool IsSet => _reset.IsSet;

        public override string ToString() {
            return $"Set={IsSet}";
        }
    }
}