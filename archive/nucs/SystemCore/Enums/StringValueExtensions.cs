using System;
using System.Collections.Generic;

namespace nucs.SystemCore.Enums {
    public static class StringValueExtensions {
        /// <summary>
        ///     Get a string value from a specific enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetStringValue<T>(this T enumValue) where T : struct, IConvertible {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            var Values = Enum.GetValues(typeof(T));
            string description = null;

            foreach (int val in Values)
                if (val == (int) (object) enumValue) {
                    var type = typeof(T);
                    var memInfo = type.GetMember(Enum.GetName(typeof(T), val));
                    var attributes = memInfo[0].GetCustomAttributes(typeof(StringValueAttribute), false);
                    description = ((StringValueAttribute) attributes[0]).Value;
                }
            return description;
        }

        /// <summary>
        ///     Gets all the string values in an enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAllStringValues<T>(this T enumValue) where T : struct, IConvertible {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            var Values = Enum.GetValues(typeof(T));

            foreach (int val in Values) {
                var attributes =
                    typeof(T).GetMember(Enum.GetName(typeof(T), val))[0].GetCustomAttributes(
                        typeof(StringValueAttribute), false);
                if (attributes.Length == 0)
                    continue;
                yield return ((StringValueAttribute) attributes[0]).Value;
            }
        }
    }

    /// <summary>
    ///     Holds a string value for an enum.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class StringValueAttribute : Attribute {

        public StringValueAttribute(string v) {
            Value = v;
        }

        /// <summary>
        ///     The value stored for this enum
        /// </summary>
        public string Value { get; }

    }
}