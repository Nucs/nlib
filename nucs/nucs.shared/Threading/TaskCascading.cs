using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Threading {
    /// <summary>
    ///     Provides ways to simplify task writing.
    /// </summary>
    public static class TaskCascading {

        public static Task AndThen(this Task task, Action act) {
            return task.ContinueWith(t => act());
        }

        public static Task AndThen<T>(this Task<T> task, Action<T> act) {
            return task.ContinueWith(t => act(t.Result));
        }

        public static Task<T> AndThen<T>(this Task task, Func<T> act) {
            return task.ContinueWith(t => act());
        }
        public static Task<TOUT> AndThen<T,TOUT>(this Task<T> task, Func<T,TOUT> act) {
            return task.ContinueWith(t => act(t.Result));
        }
    }
}