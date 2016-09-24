using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreLinq {
    public static partial class MoreEnumerable {
        /// <summary>
        ///     Return Task array from the foreach func.
        /// </summary>
        public static Task[] ForEachParallel<T>(this IEnumerable<T> source, Action<T> act) {
            return source.Select(item => System.Threading.Tasks.Task.Factory.StartNew(() => act(item))).ToArray();
        }

        /// <summary>
        ///     Return Task array from the foreach func.
        /// </summary>
        public static Task<RET>[] ForEachParallel<RET,T>(this IEnumerable<T> source, Func<T, RET> act) {
            return source.Select(item => System.Threading.Tasks.Task.Factory.StartNew(() => act(item))).ToArray();
        }
         
    }
}