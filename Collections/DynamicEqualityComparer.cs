using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIPClient
{
    public class DynamicEqualityComparer<T> : IEqualityComparer<T> {
        public delegate bool EqualsDelegate<T>(T x, T y);
        public delegate int GetHashCodeDelegate<T>(T obj);

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
