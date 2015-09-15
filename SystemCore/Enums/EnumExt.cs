#if (NET_3_5 || NET_3_0 || NET_2_0)

using System;

namespace nucs.SystemCore.Enums {

    public static class EnumExt {

        public static bool HasFlag(this Enum variable, Enum value) {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value)) {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);
        }

    }

}

#endif