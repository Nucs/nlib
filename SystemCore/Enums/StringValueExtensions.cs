using System;
using System.Collections.Generic;

namespace nucs.SystemCore.Enums {
    public static class StringValueExtensions {
        public static string GetStringValue<T>(this T enumValue) {
            Array Values = System.Enum.GetValues(typeof (T));
            string description = null;

            foreach (int val in Values) {
                if (val == (int) (object) enumValue) {
                    var type = typeof (T);
                    var memInfo = type.GetMember(Enum.GetName(typeof (T), val));
                    var attributes = memInfo[0].GetCustomAttributes(typeof (StringValueAttribute), false);
                    description = ((StringValueAttribute) attributes[0]).Value;
                }
            }
            return description;
        }

        public static IEnumerable<string> GetAllStringValues<T>(this T enumValue) {
            Array Values = System.Enum.GetValues(typeof (T));

            foreach (int val in Values) {
                var attributes =
                    typeof (T).GetMember(Enum.GetName(typeof (T), val))[0].GetCustomAttributes(
                        typeof (StringValueAttribute), false);
                if (attributes.Length == 0) continue;
                yield return ((StringValueAttribute) attributes[0]).Value;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
        public sealed class StringValueAttribute : Attribute {

        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        // This is a positional argument

        public StringValueAttribute(string v)
        {
            Value = v;
        }

        public string Value { get; private set; }

            // This is a named argument
        }
}