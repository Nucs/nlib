using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nucs.SystemCore;

namespace nucs.Collections {
    /// <summary>
    /// Identical to <see cref="Dictionary{TKey,TValue}"/> only when you try to add, if item exists it modifies it to the selected value, not throwning exception.
    /// </summary>
    public class DictionaySelfModifier<K, V> : Dictionary<K, V> {
        public new void Add(K key, V val) {
            if (ContainsKey(key)) 
                this[key] = val;
            else 
                base.Add(key, val);
            
        }

        public new V this[K key] {
            get { return base[key]; }
            set { Add(key, value); }
        }
    }


    /// <summary>
    /// Identical to <see cref="Dictionary{TKey,TValue}"/> only when you try to add, if item exists it modifies it to the selected value, not throwning exception. <remarks>Supports <see cref="AggregatableObjectBase{T}"/></remarks>
    /// </summary>
    public class DictionaySelfModifierAggregatable<K, V> : Dictionary<K, V> where V : IAggregatableObject<V> {
        public new void Add(K key, V val) {
            if (ContainsKey(key))
                this[key].Add(val);
            else
                base.Add(key, val);
        }

        public new V this[K key] {
            get { return base[key]; }
            set {
                if (ContainsKey(key) == false)
                    base.Add(key, value);
                else
                    base[key] = value;
            }
        }

        public bool Subtract(K key, V val) {
            if (ContainsKey(key)) {
                this[key].Subtract(val);
                return true;
            }
            return false;
        }
}
}
