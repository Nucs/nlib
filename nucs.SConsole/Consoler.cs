using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Reflection;
using Colorful;
using Console = Colorful.Console;

namespace nucs.SConsole {
    public static class Consoler {
        public static event Action<Exception> ExceptionThrown;

        public static bool AskQuestion(string question, bool @default) {
            _reprint:
            Console.Write("[DECIDE] ", Color.DarkOrange);
            Console.Write(question, Color.White);
            Console.Write($"{(question.Last() == ' ' ? "" : " ")}(default:[{(@default ? "y" : "n")}] - [y/n]) >", Color.Aqua);

            var r = Console.ReadLine().ToLowerInvariant();
            if (r == "y" || r == "yes" || r == "true")
                return true;
            if (r == "n" || r == "no" || r == "false" || r == "hellno" || r == "nope" || r == "nein")
                return false;
            if (string.IsNullOrEmpty(r))
                return @default;
            Error("Invalid Input");
            goto _reprint;
        }

        public static string AskInput(string question, string @default = null, Validator validate = null) {
            Func<object, bool> val;
            if (validate == null) {
                val = null;
            } else {
                val = validate.Validate;
            }
            return AskInput(question, @default, val);
        }

        public static string AskInput(string question, string @default = null, Func<object, bool> validate = null) {
            return (string) AskInput(typeof(string), question, @default, @default, validate);
        }

        public static bool AskBooleanInput(string question, bool @default) {
            return AskInput<bool>(question, @default, @default.ToString(), null);
        }

        public static T AskInput<T>(string question, object @default = null, string @default_text = null, Func<object, bool> validate = null) {
            return (T) AskInput(typeof(T), question, @default, @default_text, validate);
        }

        public static Enum AskEnum(Type enumtype, string question, Enum @default, Func<object, bool> validation) {
            if (enumtype.IsEnum == false)
                throw new InvalidOperationException($"Given enum type is not an enum! ({enumtype.Name})");

            Console.Write("[INPUT] ", Color.DarkGray);
            Console.Write(question, Color.White);
            Console.Write($"{(question.Last() == ' ' ? "" : " ")}(default:[", Color.Aqua);
            Console.Write($"{ enumtype.Name}.{@default}", Color.White);
            Console.WriteLine("],[exit])", Color.Aqua);
            var vals = Enum.GetValues(enumtype).Cast<object>().OrderByDescending(o => o.ToString()).ToArray();
            for (int i = 0; i < vals.Length; i++) {
                var val = vals[i];
                Console.Write($"    [{i}] ", Color.OrangeRed);
                Console.WriteLine($"{enumtype.Name}.{val}", Color.White);
            }
            goto _skipbeep;
            _retry:
            SystemSounds.Beep.Play();
            _skipbeep:
            Console.Write(">", Color.DarkGray);
            Console.ResetColor();
            var commandid = Console.ReadLine().Trim();
            if (commandid == "q" || commandid == "exit")
                return null;
            if (string.IsNullOrEmpty(commandid)) {
                System.Console.WriteLine($"Selected default:");
                Console.WriteLine($"{enumtype.Name}.{@default}");
                return @default;
            }

            if (commandid.IsNumeric() == false) 
                goto _retry;
            
            var @out = Convert.ToInt32(commandid);
            if (@out > vals.Length || @out < 0) {
                goto _retry;
            }

            return (Enum) vals[@out];
        }
        
        public static T AskArray<T>(T[] arr, string question, T @default, string @default_text, Func<T,string> totext) {
            if (arr == null || arr.Length == 0)
                return default(T);

            Console.Write("[INPUT] ", Color.DarkGray);
            Console.Write(question, Color.White);
            Console.Write($"{(question.Last() == ' ' ? "" : " ")}(default:[", Color.Aqua);
                Console.Write($"{ @default_text}", Color.White);
                Console.WriteLine("],[exit])", Color.Aqua);
            for (int i = 0; i < arr.Length; i++) {
                var val = arr[i];
                Console.Write($"    [{i}] ", Color.OrangeRed);
                Console.WriteLine($"{totext(val)}", Color.White);
            }
            goto _skipbeep;
            _retry:
            SystemSounds.Beep.Play();
            _skipbeep:
            Console.Write(">", Color.DarkGray);
            Console.ResetColor();
            var commandid = Console.ReadLine().Trim();
            if (commandid == "q" || commandid == "exit")
                return default(T);
            if (string.IsNullOrEmpty(commandid)) {
                System.Console.WriteLine($"Selected default:");
                Console.WriteLine($"{ @default_text}", Color.White);
                return @default;
            }

            if (commandid.IsNumeric() == false) 
                goto _retry;
            
            var @out = Convert.ToInt32(commandid);
            if (@out > arr.Length || @out < 0) {
                goto _retry;
            }

            return arr[@out];
        }
        private static IEnumerable<T> GetEnumValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="out_type">The type that will be parsed</param>
        /// <param name="question">The question that will be presented to the console</param>
        /// <param name="default">The default value if input is empty</param>
        /// <param name="default_text">The text that explains the default object</param>
        /// <param name="validate">The validator object</param>
        /// <returns></returns>
        public static object AskInput(Type @out_type, string question, object @default = null, string @default_text = null, Func<object, bool> validate = null) {
            _reprint:
            Console.Write("[INPUT] ", Color.DarkGray);
            Console.Write(question, Color.White);
            Console.WriteLine($"{(question.Last() == ' ' ? "" : " ")}(default:[{@default_text ?? "Null"}])", Color.Aqua);

            Console.Write(">", Color.DarkGray);
            Console.ResetColor();
            var input = Console.ReadLine()?.ExpandEscaped() ?? "";

            if (string.IsNullOrEmpty(input))
                return @default;

            var @out = MapToType(input, @out_type);
            if (@out is Exception && (@out as Exception).Message == "__ERROR") {
                //invalid cast!
                Consoler.Error($"Failed casting to '{out_type.Name}' type.");
                goto _reprint;
            }

            if (validate != null) {
                if (!validate(@out)) {
                    Error("Invalid Input");
                    goto _reprint;
                }
            }
            return @out;
        }

        private static object MapToType(string str, Type @out_type) {
            if (string.IsNullOrEmpty(str))
                return null;

            if (stringToTypeMap.ContainsKey(@out_type)) {
                return stringToTypeMap[@out_type](str);
            }

            try {
                return Convert.ChangeType(str, @out_type, CultureInfo.InvariantCulture);
            } catch (InvalidCastException) {}
            return new Exception("__ERROR");
        }

        private static readonly Dictionary<Type, Func<string, object>> stringToTypeMap = new Dictionary<Type, Func<string, object>> {
            {typeof(string), s => s},
            {typeof(DateTime), s => DateTime.Parse(s, CultureInfo.InvariantCulture)}, {
                typeof(char), s => {
                    s = s.ExpandEscaped();
                    if (s.Length != 1)
                        throw new InvalidCastException($"Cannot cast string \"{s}\" to char.");
                    return s[0];
                }
            }, {
                typeof(bool), r => {
                    if (r == "y" || r == "yes" || r == "true")
                        return true;
                    if (r == "n" || r == "no" || r == "false" || r == "hellno" || r == "nope" || r == "nein")
                        return false;
                    throw new InvalidCastException($"Cannot cast string \"{r}\" to boolean.");
                }
            },
        };

        private static string ExpandEscaped(this string str) {
            return str.Replace("\\t", "\t").Replace("\\0", "\0").Replace("\\n", "\n").Replace("\\r", "\r");
        }

        /// <summary>
        ///     Prints an exception
        /// </summary>
        public static void Error(string msg) {
            Log("ERROR", Color.Red, msg);
        }

        /// <summary>
        ///     Prints an exception
        /// </summary>
        public static void Error(Exception e) {
#if DEBUG
            Error(e.ToString());
#else
            Error(e.Message);
#endif
            ExceptionThrown?.Invoke(e);
        }

        /// <summary>
        /// Logs with color and type for example: [ERROR] message
        /// </summary>
        /// <param name="type">for [ERROR] type ERROR</param>
        /// <param name="color">Color is for the [ERROR]</param>
        /// <param name="message">The message</param>
        public static void Log(string type, Color color, string message) {
            Console.Write($"[{type}] ", color);
            Console.WriteLine(message, Color.White);
        }
    }
}