using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
namespace nucs.Collections {

    /// <summary>
    /// Unique key and value with event system, no validation.
    /// </summary>
    public class EventyDictionary<TKey,TValue> {
        public delegate void DictonaryItemRemovedHandler(TKey key, TValue value);
        public delegate void DictonaryItemAddedHandler(TKey key, TValue value);

        public event DictonaryItemAddedHandler OnAddedItemEvent;
        public event DictonaryItemRemovedHandler OnRemovedItemEvent;

        private Dictionary<TKey, TValue> dic { get { return dicholder; } set { dicholder = value;
            OnAddedItemEvent += (key, value1) => { };
            OnRemovedItemEvent += (key, value1) => { };

        } }
        private Dictionary<TKey, TValue> dicholder; 
        public TValue this[TKey key] { get { return dic[key]; } set { dic[key] = value; } }

        public EventyDictionary(IDictionary<TKey, TValue> dictionary) {dic = new Dictionary<TKey, TValue>(dictionary);}
        public EventyDictionary(Dictionary<TKey, TValue> dictionary) { dic = dictionary; } 
        public EventyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) {dic = new Dictionary<TKey, TValue>(dictionary, comparer);} 
        public EventyDictionary(IEqualityComparer<TKey> comparer) {dic = new Dictionary<TKey, TValue>(comparer);} 
        public EventyDictionary(int capacity) {dic = new Dictionary<TKey, TValue>(capacity);}
        public EventyDictionary(int capacity, IEqualityComparer<TKey> comparer) { dic = new Dictionary<TKey, TValue>(capacity, comparer);} 
        public EventyDictionary() {dic = new Dictionary<TKey, TValue>();}

        public bool Add(TKey key, TValue val) {
            if (!dicholder.Keys.Any(k => key.Equals(k)) && !dicholder.Values.Any(v => val.Equals(v))) {
                dic.Add(key, val);
                OnAddedItemEvent(key, val);
                return true;
            }
            return false;
        }
        public bool Remove(TKey key) {
            if (dic.ContainsKey(key)) {
                TValue val = dic[key];
                dic.Remove(key);
                OnRemovedItemEvent(key, val);
                return true;
            }
            return false;
        }


        public bool Remove(TValue val) {
            var key = TryGetKey(val);
            if (key != null) {
                dic.Remove(key);
                OnRemovedItemEvent(key, val);
                return true;
            }
            return false;
        }
        public bool ContainsKey(TKey key) { return dic.ContainsKey(key); }
        public bool ContainsKey(TValue value) { return dic.ContainsValue(value); }
        public int Count() { return dic.Count; }
        public Dictionary<TKey, TValue>.Enumerator GetEnumerator() { return dic.GetEnumerator(); }
        public Dictionary<TKey, TValue>.KeyCollection Keys() { return dic.Keys; }
        public Dictionary<TKey, TValue>.ValueCollection Values() { return dic.Values; }
        public void GetObjectData(SerializationInfo info, StreamingContext context) {dic.GetObjectData(info, context); }
        public void OnDeserialization(object sender) { dic.OnDeserialization(sender); }
        public IEqualityComparer<TKey> Comparer() { return dic.Comparer; }
        public void TryGetValue(TKey key, out TValue value) { dic.TryGetValue(key, out value); }
        public void Clear(TKey key, TValue val) {dic.Clear();}
        public TKey TryGetKey(TValue val) { 
            var keys = dicholder.Where(pair => pair.Value.Equals(val)).Select(p => p.Key).ToArray();
            if (keys.Length == 0)
                return dicholder.FirstOrDefault(p=>p.Value == null).Key; //null...
            if (keys.Length > 1)
                throw new InvalidOperationException("Too many keys were found by one value!");
            return keys[0];
        }


        public static implicit operator Dictionary<TKey, TValue>(EventyDictionary<TKey, TValue> dic) {
            return dic.dicholder;
        } 

        public static implicit operator EventyDictionary<TKey, TValue>(Dictionary<TKey, TValue> dic) {
            return new EventyDictionary<TKey, TValue>(dic);
        } 

    }

}