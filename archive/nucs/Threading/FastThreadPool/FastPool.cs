using System;
using System.Threading.Tasks;

namespace nucs.Threading.FastThreadPool {
    /// <summary>
    ///     Static singleton implementation of FastThreadPool, once accessed - a static constuctor initializes a new instance.
    /// </summary>
    public static class FastPool {
        static FastPool() {
            FastThreadPool.Default = new FastThreadPool(8);
        }

        /// <summary>
        ///     Enqueues a job
        /// </summary>
        /// <param name="act"></param>
        public static void Enqueue(Action act) {
            FastThreadPool.Default.Enqueue(act);
        }

        public static Task<T> Enqueue<T>(Func<T> act) {
            return FastThreadPool.Default.Enqueue(act);
        }

        /// <summary>
        ///     Enqueue a monitored job (wraps the action with ManualResetEventSlim
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public static IWaitable EnqueueMonitored(Action act) {
            return FastThreadPool.Default.EnqueueMonitored(act);
        }
    }
}