using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var forEach = new List<T>();
            foreach (var j in list) {
                forEach.Add(j);
                action.Invoke(j);
            }
            return forEach;
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


    }
}
