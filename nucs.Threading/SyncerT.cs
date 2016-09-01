using System;
using System.Threading;

namespace nlib.Threading.FastThreadPool {
    public class Syncer<T> : IDisposable {
        private readonly ManualResetEventSlim _reset = new ManualResetEventSlim();
        private T obj;
        private readonly object syncer = new object();
        public void Set(T o) {
            lock (syncer) {
                obj = o;
                _reset.Set();
            }
        }

        public void Reset() {
            lock (syncer) {
                obj = default(T);
                _reset.Reset();
            }
        }

        public T Wait() {
            _reset.Wait();
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
            return $"Set={IsSet}, Item={obj}";
        }
    }
}