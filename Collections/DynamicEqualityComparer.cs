using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIPClient
{
    public delegate bool EqualsDelegate<in T>(T x, T y);
    public delegate int GetHashCodeDelegate<in T>(T obj);
    public class DynamicEqualityComparer<T> : IEqualityComparer<T> {


        public EqualsDelegate<T> EqualityMethod;
        public GetHashCodeDelegate<T> GetHashCodeMethod;

        public DynamicEqualityComparer(EqualsDelegate<T> equalityMethod, GetHashCodeDelegate<T> hashcodeMethod) {
            EqualityMethod = equalityMethod;
            GetHashCodeMethod = hashcodeMethod;
        } 

        public bool Equals(T x, T y) {
            return EqualityMethod(x, y);
        }

        public int GetHashCode(T obj) {
            return GetHashCodeMethod(obj);
        }
    }
}
