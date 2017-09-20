using System;

namespace TFer.Commons {
    public static class EnumExtension {
        public static T FromEnumObj<T>(this Enum e) {
            if (typeof (T).IsEnum == false || e == null) return (T)(object)-1;
            return (T)Enum.Parse(typeof(T), e.ToString());
        }

        public static int ToInt(this Enum e) {
            if (e == null) return -1;
            return (int) (object) e;
        }

        public static int ToInt(this object e) {
            if (e == null) return -1;
            return (int) (object) e;
        }

        public static T FromInt<T>(this int e) {
            return (T) Enum.ToObject(typeof (T), e);
        }

        public static Enum FromIntToEnumObj<T>(this int e) {
            return (Enum) Enum.ToObject(typeof (T), e);
        }

        public static Enum ToEnumObj<T>(this T e) {
            return (Enum)(object)e;
        }
        public static T ToEnum<T>(this object o) {
            if (o == null) return (T) (object) -1;
            return (T)o;
        }

#if (NET35 || NET3 || NET2)

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
#endif
    }
}