#if NET4_5|| NET4_0
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.Collections
{
    public class ConcurrentDict<K, V> : ConcurrentDictionary<K, V> {
        public bool Remove(K key) {
            V val;
            return TryRemove(key, out val);
        }

        public void Add(K key, V val) {
            TryAdd(key, val);
        }

        public void Add(KeyValuePair<K,V> kv) {
            TryAdd(kv.Key, kv.Value);
        }

        public void AddRange(IEnumerable<K> Ks, IEnumerable<V> Vs) {
            var kvs = from k in Ks from v in Vs select new KeyValuePair<K, V>(k, v);
            kvs.AsParallel().ForAll(Add);
        }

        

    }
}
#endif