using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;

namespace nucs.SConsole {
    public static class SConsoleApp {
        public static void Start() {
            SConsole.PrintMethods();
            while (true) {
                var e = SConsole.CaptureCommand(false,false);
                if (e!=null)
                    Console.WriteLine(e);
            }
        }

        public static void Start(string title) {
            try {
                Console.Title = title;
            } catch (IOException) { }
            Start();
        }
    }

    public static class SConsole {
        public static List<MethodInfo> Actions = new List<MethodInfo>();

        static SConsole() {
            //load consoleactions
            var methods = AppDomain.CurrentDomain.GetAssemblies().OrderByDescending(asm=>asm.Equals(Assembly.GetExecutingAssembly())).SelectMany(asm => asm.GetTypes())
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(ConsoleMethodAttribute), false).Length > 0)
                .Where(m => m.GetParameters().All(p => p.ParameterType == typeof(string)))
                .ToArray();
            Actions.AddRange(methods);
        }

        public static void PrintMethods() {
            var acts = Actions.Select(a => 
            ((ConsoleMethodAttribute)Attribute.GetCustomAttribute(a, typeof(ConsoleMethodAttribute)))?.CustomName ?? $"{(a.ReturnType.Name == "Void" ? "" : (a.ReturnType.Name + " "))}{a.DeclaringType.Name}.{a.Name}")
                .ToArray();
            var methods = Actions
                .Select(a => new {att= ((ConsoleMethodAttribute)Attribute.GetCustomAttribute(a, typeof(ConsoleMethodAttribute))) , p = a.GetParameters()})
                .Select(c => c.att.PrintParamteres ? c.p.Select(pp => $"{pp.ParameterType.Name} {pp.Name}").ToArray():new string[0])
                .ToArray();
            for (var i = 0; i < acts.Length; i++) {
                var b = string.Join(", ", methods[i]);
                if (string.IsNullOrEmpty(b) == false)
                    b = $"({b})";
                
                Console.WriteLine($"[{i}]{(acts.Length>=11? (i<10 ? "  ": " "):" ")}{acts[i]}{b}");
            }
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
            Console.Write(">");
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
            var @params = act.GetParameters();
            if (@params.Length != args.Length || act.GetParameters().All(p => p.ParameterType == typeof(string)) == false) {
                goto _retry;
            }

            try {
                var ret = act.Invoke(null, args.Cast<object>().ToArray());
                if (ret != null)
                    Console.WriteLine(ret);
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

        private static bool IsNumeric(this string Expression) {
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
}