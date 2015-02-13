using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.SystemCore.AttachedObjects {
    public class TriObject<T1,T2,T3> {
        
        public T1 First { get; set; }
        
        public T2 Second { get; set; }
        public T3 Third { get; set; }
        
        public TriObject(T1 first, T2 second, T3 third) {
            First = first;
            Second = second;
            Third = third;
        } 

        public TriObject(){}

        protected bool Equals(TriObject<T1, T2, T3> other) {
            return EqualityComparer<T1>.Default.Equals(First, other.First) && EqualityComparer<T2>.Default.Equals(Second, other.Second) && EqualityComparer<T3>.Default.Equals(Third, other.Third);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TriObject<T1, T2, T3>) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = EqualityComparer<T1>.Default.GetHashCode(First);
                hashCode = (hashCode*397) ^ EqualityComparer<T2>.Default.GetHashCode(Second);
                hashCode = (hashCode*397) ^ EqualityComparer<T3>.Default.GetHashCode(Third);
                return hashCode;
            }
        }
    }
}
