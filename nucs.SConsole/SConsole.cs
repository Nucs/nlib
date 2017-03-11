using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using Console = Colorful.Console;

namespace nucs.SConsole {
    public static class SConsoleApp {
        public static event Action OnPrintedHelp;
        public static event Action OnPrePrintedHelp;
        public static void Start() {
            SConsole.PrintMethods();
            while (true) {
                var e = SConsole.CaptureCommand(false,false);
                if (e!=null)
                    Console.WriteLine(e, Color.White);
            }
        }

        public static void Start(string title) {
            try {
                Console.Title = title;
            } catch (IOException) { }
            Start();
        }

        internal static void CallOnPrintedHelp() => OnPrintedHelp?.Invoke();
        internal static void CallOnPrePrintedHelp() => OnPrePrintedHelp?.Invoke();
    }

    public static class SConsole {
        public static List<MethodInfo> Actions = new List<MethodInfo>();

        static SConsole() {
            //load consoleactions
            var methods = AppDomain.CurrentDomain.GetAssemblies().OrderByDescending(asm=>asm.Equals(Assembly.GetExecutingAssembly())).SelectMany(asm => asm.GetTypes())
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(ConsoleMethodAttribute), false).Length > 0)
                .Where(m => m.GetParameters().All(p => p.ParameterType == typeof(string) || (p.ParameterType != typeof(string) && p.GetCustomAttributes(typeof(ConsoleParamAttribute)).Any())))
                .ToArray();
            Actions.AddRange(methods);
        }

        public static void PrintMethods() {
            SConsoleApp.CallOnPrePrintedHelp();
            var acts = Actions.Select(a =>
                a.GetCustomAttributes<ConsoleMethodAttribute>()?.FirstOrDefault()?.CustomName
                ?? $"{(a.ReturnType.Name == "Void" ? "" : (a.ReturnType.Name + " "))}{a.DeclaringType.Name}.{a.Name}")
                .ToArray();
            var methods = Actions
                .Select(a => new {att= a.GetCustomAttributes<ConsoleMethodAttribute>()?.FirstOrDefault() , p = a.GetParameters()})
                .Select(c => c.att.PrintParamteres ? c.p.Select(pp => $"{pp.ParameterType.Name} {pp.Name}").ToArray():new string[0])
                .ToArray();
            for (var i = 0; i < acts.Length; i++) {
                var b = string.Join(", ", methods[i]);
                if (string.IsNullOrEmpty(b) == false)
                    b = $"({b})";
                
                Console.WriteLine($"[{i}]{(acts.Length>=11? (i<10 ? "  ": " "):" ")}{acts[i]}{b}", Color.White);
            }
            SConsoleApp.CallOnPrintedHelp();
            Console.WriteLine();

        }

        public static Exception CaptureCommand(bool cls = false, bool print=false) {
            if (cls)
                Console.Clear();
            if (print)
                PrintMethods();
            goto _skipbeep;

            _retry:
            SystemSounds.Beep.Play();
            _skipbeep:
            Console.Write(">", Color.DarkGray);
            Console.ResetColor();
            var l = ReadLine().Trim();

            if (string.IsNullOrEmpty(l)) {
                goto _retry;
            }

            var splet = l.Split("\t", StringSplitOptions.RemoveEmptyEntries);

            var commandid = splet[0];
            if (commandid.IsNumeric() == false) {
                goto _retry;
            }
            var args = splet.Skip(1).ToArray();
            MethodInfo act;
            try {
                act = Actions[Convert.ToInt32(commandid)];
            } catch {
                goto _retry;
            }
            List<object> collected = null;
            var @params = act.GetParameters();
            if (@params.Length != args.Length || @params.All(p => p.ParameterType == typeof(string)) == false) {

                //check if has ConsoleParam
                if (@params.All(p => p.GetCustomAttributes(typeof(ConsoleParamAttribute), false).Any()) == false) //validate that all params has this option
                    goto _retry;
                collected = new List<object>();
                foreach (var para in @params) {
                    var attr = para.GetCustomAttributes(typeof(ConsoleParamAttribute), false).FirstOrDefault() as ConsoleParamAttribute;
                    if (attr == null)
                        goto _retry;
                    
                    var val = attr.Validator == null ? (Func<object, bool>) null : attr.Validator.Validate;
                    if (para.ParameterType.IsEnum) {
                        collected.Add(Consoler.AskEnum(para.ParameterType, attr.Question, (Enum)attr.Default, val));
                    } else {
                        collected.Add(Consoler.AskInput(para.ParameterType, attr.Question, attr.Default, attr.Default?.ToString(), val));
                    }

                }
                if (@params.Length != collected.Count) {
                    Consoler.Error("Something wen't wrong during parameters collecting.");
                    goto _retry;
                }
            }

            try {
                var ret = act.Invoke(null, collected?.ToArray() ?? args.Cast<object>().ToArray());
                if (ret != null)
                    Console.WriteLine(ret, Color.CornflowerBlue);
            } catch (Exception e) {
                return e;
            }
            return null;
        }

        public static string ReadLine() {
            var rl = Console.ReadLine();
            return rl;
        }


        [ConsoleMethod("?")]
        public static void Help() {
            PrintMethods();
        }

        [ConsoleMethod("Clear")]
        public static void Clear() {
            Console.Clear();
            Help();
        }

        public static bool IsNumeric(this string Expression) {
            double retNum;
            return double.TryParse(Expression, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
        }

        private static string[] Split(this string @this, string separator, StringSplitOptions option = StringSplitOptions.None) {
            return @this.Split(new[] {separator}, option);
        }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ConsoleMethodAttribute : Attribute {
        public ConsoleMethodAttribute(string customName) {
            CustomName = customName;
        }

        public ConsoleMethodAttribute() {}
        public string CustomName { get; set; }
        public bool PrintParamteres { get; set; } = true;
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class ConsoleParamAttribute : Attribute {
        public ConsoleParamAttribute(string question, object @default = null, Type validator = null) {
            Question = question;
            Default = @default;
            
            if (validator!=null)
                Validator = Activator.CreateInstance(validator) as Validator;
        }

        /// <summary>
        ///     The question asked for this parameter
        /// </summary>
        public string Question { get; set; }

        public object Default { get; set; }
        public Validator Validator { get; set; }
    }

    public abstract class Validator {
        public abstract bool Validate(object input);
    }
}