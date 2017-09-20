using System.Collections.Generic;

namespace nucs.Collections {
    public interface ICollectorAdder<T> {
        void Add(T item, bool isLast = false);
        void AddRange(IEnumerable<T> items, bool isLast = false);
    }
}