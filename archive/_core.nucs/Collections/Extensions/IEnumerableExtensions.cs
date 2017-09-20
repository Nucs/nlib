using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.Collections.Extensions {
    public static class IEnumerableExtensions {
        
        public static ImprovedList<T> ToImprList<T>(this IEnumerable<T> l) {
            if (l is ImprovedList<T>)
                return (ImprovedList<T>) l;
            return new ImprovedList<T>(l);
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> kvs) {
            return kvs.ToDictionary(kv => kv.Key, kv => kv.Value);
        } 

        public static int Length<T>(this IEnumerable<T> list) {
            return list.Count();
        } 

        public static ArrayList ToArrayList<T>(this IEnumerable<T> list) {
            return new ArrayList(list.ToArray());
        } 

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action) {
            foreach (var j in list) {
                action.Invoke(j);
                yield return j;
            }
        }
        public static IEnumerable<T> ForEachSafe<T>(this IEnumerable<T> list, Action<T> action) {
            foreach (var j in list) {
                try {
                    action.Invoke(j);
                } catch {}
                yield return j;
            }
        }

        public static IEnumerable<T> ForEachSelf<T>(this IEnumerable<T> list, Action<T[], T> action, object unknown) {
            var forEach = list as T[] ?? list.ToArray();
            foreach (var j in forEach) {
                action.Invoke(forEach, j);
            }
            return forEach;
        }

        public static List<T> ForEach<T>(this List<T> list, Action<T> action) {
            var forEach = new List<T>();
            foreach (var j in list) {
                forEach.Add(j);
                action.Invoke(j);
            }
            return forEach;
        } 

        public static Array ForEach<T>(this T[] list, Action<T> action) {
            foreach (var j in list) {
                action.Invoke(j);
            }
            return list;
        }
        public static Array ForEachSafe<T>(this T[] list, Action<T> action) {
            foreach (var j in list) {
                try {
                    action.Invoke(j);
                } catch {}
            }
            return list;
        }

        public delegate bool CombineComparer<in K, in V>(K key, V val);
        public static IEnumerable<KeyValuePair<K, V>> Combine<K,V>(this IEnumerable<K> a, IEnumerable<V> b, CombineComparer<K, V> comparer) {
            var _a = a.ToList();
            var _b = b.ToList();
            for (int i = 0; i < _a.Count; i++) {
                var k = _a[i];
                for (int j = 0; j < _b.Count; j++) {
                    var v = _b[j];
                    if (comparer(k, v))
                        yield return new KeyValuePair<K, V>(k, v);

                }
            }
        }

        /// <summary>
        /// Simple way to convert an single item to an IEnumerable and eventually any type of collection.
        /// </summary>
        public static IEnumerable<T> ToEnumerable<T>(this T obj) {
            yield return obj;
        }

        /// <summary>
        ///     Does action, used to do something in a fluent typin
        /// </summary>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> list, Action<T[]> action) {
            var l = list.ToArray();
            action(l);
            return l;
        }

        public static void EvaluateLinq<T>(this IEnumerable<T> list) {
            var e = list.GetEnumerator();
            while (e.MoveNext()) ;
        }

        public delegate TT ObjectCascader<T, TT>(T obj);
        /// <summary>
        ///     If <paramref name="obj"/> is the default value, returns the default, otherwise calls the action and returns the result.
        /// </summary>
        public static TT IfNotNull<T, TT>(this T obj, ObjectCascader<T, TT> act) {
            if (Equals(obj, default(T)))
                return default(TT);
            return act(obj);
        }

        /// <summary>
        ///     Filters all null values using .equals
        /// </summary>
        public static IEnumerable<T> FilterNulls<T>(this IEnumerable<T> list) {
            if (list == null) {
                yield break;
            }

            foreach (var t in list.Where(t => t.Equals(null) == false))
                yield return t;
        }

        public static IEnumerable<T> PrintEach<T>(this IEnumerable<T> list) {
            foreach (var obj in list) {
                Console.WriteLine(obj);
                yield return obj;
            }
        } 

        public static IEnumerable<T> PrintEachIfDebug<T>(this IEnumerable<T> list) {

            foreach (var obj in list) {
#if DEBUG
                Console.WriteLine(obj);
#endif
                yield return obj;
            }
        } 
    }
}
