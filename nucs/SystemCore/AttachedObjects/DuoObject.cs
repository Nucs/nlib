using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nucs.SystemCore.AttachedObjects {
    public class DuoObject<T1,T2> {
        
        public T1 First { get; set; }
        
        public T2 Second { get; set; }
        
        public DuoObject(T1 first, T2 second) {
            First = first;
            Second = second;
        } 

        public DuoObject(){}

        protected bool Equals(DuoObject<T1, T2> other) {
            return EqualityComparer<T1>.Default.Equals(First, other.First) && EqualityComparer<T2>.Default.Equals(Second, other.Second);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DuoObject<T1, T2>) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (EqualityComparer<T1>.Default.GetHashCode(First)*397) ^ EqualityComparer<T2>.Default.GetHashCode(Second);
            }
        }
    }
}
