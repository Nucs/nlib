using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.SystemCore.AttachedObjects {
    public class PentaObject<T1,T2,T3,T4,T5> {
        
        public T1 First { get; set; }
        
        public T2 Second { get; set; }
        public T3 Third { get; set; }
        public T4 Fourth { get; set; }
        public T5 Fifth { get; set; }
        
        
        public PentaObject(T1 first, T2 second, T3 third, T4 fourth, T5 fifth) {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;
        }

        public PentaObject() { }

        protected bool Equals(PentaObject<T1, T2, T3, T4, T5> other) {
            return EqualityComparer<T1>.Default.Equals(First, other.First) && EqualityComparer<T2>.Default.Equals(Second, other.Second) && EqualityComparer<T3>.Default.Equals(Third, other.Third) && EqualityComparer<T4>.Default.Equals(Fourth, other.Fourth) && EqualityComparer<T5>.Default.Equals(Fifth, other.Fifth);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PentaObject<T1, T2, T3, T4, T5>) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = EqualityComparer<T1>.Default.GetHashCode(First);
                hashCode = (hashCode*397) ^ EqualityComparer<T2>.Default.GetHashCode(Second);
                hashCode = (hashCode*397) ^ EqualityComparer<T3>.Default.GetHashCode(Third);
                hashCode = (hashCode*397) ^ EqualityComparer<T4>.Default.GetHashCode(Fourth);
                hashCode = (hashCode*397) ^ EqualityComparer<T5>.Default.GetHashCode(Fifth);
                return hashCode;
            }
        }
    }
}
