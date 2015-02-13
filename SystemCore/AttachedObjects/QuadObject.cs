using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.SystemCore.AttachedObjects {
    public class QuadObject<T1,T2,T3,T4> {
        
        public T1 First { get; set; }
        
        public T2 Second { get; set; }
        public T3 Third { get; set; }
        public T4 Fourth { get; set; }
        
        public QuadObject(T1 first, T2 second, T3 third, T4 fourth) {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }

        public QuadObject() { }

        protected bool Equals(QuadObject<T1, T2, T3, T4> other) {
            return EqualityComparer<T1>.Default.Equals(First, other.First) && EqualityComparer<T2>.Default.Equals(Second, other.Second) && EqualityComparer<T3>.Default.Equals(Third, other.Third) && EqualityComparer<T4>.Default.Equals(Fourth, other.Fourth);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((QuadObject<T1, T2, T3, T4>) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = EqualityComparer<T1>.Default.GetHashCode(First);
                hashCode = (hashCode*397) ^ EqualityComparer<T2>.Default.GetHashCode(Second);
                hashCode = (hashCode*397) ^ EqualityComparer<T3>.Default.GetHashCode(Third);
                hashCode = (hashCode*397) ^ EqualityComparer<T4>.Default.GetHashCode(Fourth);
                return hashCode;
            }
        }
    }
}
