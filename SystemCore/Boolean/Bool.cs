using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nucs.SystemCore {
    /// <summary>
    /// A simple class that holds boolean value.<remarks>You can implicit to Boolean and explicit boolean to Bool</remarks>
    /// </summary>
    public class Bool {
        public bool value = false; //default

        public Bool(bool b) {
            value = b;
        }

        public Bool(Bool b) {
            value = b.value;
        }

        public static explicit operator Bool(bool b) {
            return new Bool(b);
        }

        public static implicit operator bool(Bool b) {
            return b.value;
        }

        public static bool EqualsAny(object obj, params object[] objs) {
            return objs.Any(o => o.Equals(obj));
        }

        public static bool EqualsAny<T>(T obj, params T[] objs) {
            return objs.Any(o => o.Equals(obj));
        }
    }
}
