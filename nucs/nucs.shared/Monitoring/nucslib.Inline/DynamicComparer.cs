using System;
using System.Collections;
using System.Collections.Generic;

namespace nucs.Comparers {

    internal delegate int CompareHandler<in T>(T x, T y);

    public delegate bool CompareEqualityHandler<in T>(T x, T y);

    public delegate int HashcodeHandler<in T>(T x);


    /// <summary>
    /// Provides a base class for implementations of the <see cref="T:System.Collections.Generic.IComparer`1"/> generic interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam><filterpriority>1</filterpriority>
    [Serializable]
    internal class DynamicComparer<T> : Comparer<T> {

        public CompareHandler<T> ComparingMethod { get; private set; }

        public DynamicComparer(CompareHandler<T> comparer) {
            ComparingMethod = comparer;
        }

        public override int Compare(T x, T y) {
            return this.ComparingMethod(x, y);
        }
    }

    /// <summary>
    /// Provides a base class for implementations of the <see cref="T:System.Collections.Generic.IComparer`1"/> generic interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam><filterpriority>1</filterpriority>
    [Serializable]
    public class DynamicEqualityComparer<T> : EqualityComparer<T> {

        public CompareEqualityHandler<T> CompareEqualityMethod { get; private set; }
        public HashcodeHandler<T> HashcodeMethod { get; private set; }

        public DynamicEqualityComparer(CompareEqualityHandler<T> comparer, HashcodeHandler<T> hashcalc) {
            CompareEqualityMethod = comparer;
            HashcodeMethod = hashcalc;
        }

        public override bool Equals(T x, T y) {
            return CompareEqualityMethod(x, y);
        }

        public override int GetHashCode(T obj) {
            return HashcodeMethod(obj);
        }
    }
}