using System;
using System.Collections.Generic;
using System.Text;

namespace nucs.Collections
{
    /// <summary>
    ///     A dictionary to always return results, if doesnt exist - default(T) will be returned
    /// </summary>
    public class SafeDictionary<TKey,TValue> : Dictionary<TKey, TValue> {
        public TValue CustomDefault = default(TValue);
        public new TValue this[TKey key] {
            get {
                if (ContainsKey(key) == false)
                    return CustomDefault;
                return base[key];
            }
            set { base[key] = value; }
        }
    }
}
