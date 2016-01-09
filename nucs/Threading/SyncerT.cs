using System;
using System.Threading;

namespace nucs.Threading {

    /// <summary>
    ///     Syncs object retrival using ManualResetEventSlim style.
    /// </summary>
    public class Syncer<T> : IDisposable {
        private readonly ManualResetEventSlim _reset = new ManualResetEventSlim();
        private T obj;
        private readonly object syncer = new object();

        /// <summary>
        ///     Root locker for every action 
        /// </summary>
        public object Root => syncer;

        /// <summary>
        ///     Sets the object 
        /// </summary>
        /// <param name="o"></param>
        public void Set(T o) {
            lock (syncer) {
                if (IsSet)
                    throw new InvalidOperationException("Syncer is already set, reset it before setting it");
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


        /// <summary>
        ///     Waits for a result to arrive, if its already set then returns the object.
        /// </summary>
        public T Wait() {
            _reset.Wait();
            return obj;
        }
        /// <summary>
        ///     Waits for a result to arrive, if its already set then returns the object.
        /// </summary>
        public T Wait(CancellationToken token) {
            _reset.Wait(token);
            return obj;
        }

        /// <summary>
        ///     Waits for a result to arrive, if its already set then returns the object.
        ///     If timesout then returns default(T)
        /// </summary>
        public T Wait(TimeSpan timeout) {
            if (!_reset.Wait(timeout))
                return default(T);
            return obj;
        }

        /// <summary>
        ///     Waits for a result to arrive, if its already set then returns the object.
        ///     If timesout then returns default(T)
        /// </summary>
        public T Wait(TimeSpan timeout, CancellationToken token) {
            if (!_reset.Wait(timeout, token))
                return default(T);
            return obj;
        }

        /// <summary>
        ///     Waits for a result to arrive, if its already set then returns the object.
        ///     If timesout then returns default(T)
        /// </summary>
        public T Wait(int millisecondsTimeout) {
            if (!_reset.Wait(millisecondsTimeout))
                return default(T);
            return obj;
        }
        /// <summary>
        ///     Waits for a result to arrive, if its already set then returns the object.
        ///     If timesout then returns default(T)
        /// </summary>
        public T Wait(int millisecondsTimeout, CancellationToken token) {
            if (!_reset.Wait(millisecondsTimeout,token))
                return default(T);
            return obj;
        }
        public void Dispose() {
            _reset.Dispose();
            obj = default(T); //release reference
        }
        
        /// <summary>
        ///     Attempts to take the result, if the syncer is not set then default(T) will be returned.
        /// </summary>
        public T Take() {
            return Wait(0);
        }

        public bool IsSet => _reset.IsSet;

        public override string ToString() {
            return $"Set={IsSet}, Item={obj}";
        }
    }
}