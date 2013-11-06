using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using nucs.Collections.Extensions;

namespace nucs.Windows.ConsoleUtils {
    public static class Consoler {
        private static Thread reader;
        private static Dictionary<string, MethodInfo> types;

        private static bool started = false;
        public static void BeginReading(Assembly assembly, bool ConsoleExists, bool InterduceTechnology) {
            if (started)
                return;
            if (!ConsoleExists)
                ConsoleHelper.StartConsole();
            started = true;
            var _types = GetConsolerMethods(assembly).ToList();
            types = new Dictionary<string, MethodInfo>();
            InitiateBasicFunctions();
            foreach (var t in _types)
                types.Add(t.Name, t);
            reader = new Thread(() => Reader(InterduceTechnology));
            reader.Start();
        }

        
        private static void Reader(bool interduce) {
            if (interduce) {
                Console.WriteLine();
                Console.WriteLine("NUCS Consoler "+FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            }

            while (true) {
                Console.Write(">");
                var comm = Console.ReadLine();
                if (string.IsNullOrEmpty(comm)) continue;
                var item = types.FirstOrDefault(jk => jk.Key.ToLowerInvariant() == comm.ToLowerInvariant());
                if (item.Value == null) {
                    Console.WriteLine("Couldn't find the method");
                    continue;
                }
                item.Value.Invoke(null, null);
            }
        }

        [ConsolerMethod]
        public static void Stop() {
            reader.Abort();
        }

        #region Nested type: ConsoleMethod

        internal static void InitiateBasicFunctions() {
            if (types == null)
                types = new Dictionary<string, MethodInfo>();
            var m = typeof (ConsolerCommands).GetMethods(BindingFlags.Static | BindingFlags.Public).ToList();
            m.ForEach(mm => types.Add(mm.Name, mm));
        }

        internal static IEnumerable<MethodInfo> GetConsolerMethods(Assembly assembly) {
            var s = new List<MethodInfo>();
            var p = assembly.GetTypes().Select(j => j.GetMethods().Where(m=>m.GetCustomAttribute(typeof(ConsolerMethod)) != null)).ToList();
            p.ForEach(s.AddRange);
            p.Clear();
            return s;
        }

        #endregion

        internal class ConsolerCommands {
            public static void Clear() {
                Console.Clear();
                Console.WriteLine("NUCS Consoler " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            }

            internal static string helpCommand = null;

            [ConsolerMethod("available commands")]
            public static void Help() {
                if (helpCommand != null) {
                    Console.WriteLine(helpCommand);
                    return;
                }
                    
                var strings = new List<string>();
                foreach (var t in types.Select(k=>k.Value)) {
                    //.Select(jk=>("-"+jk.Key + " - " + (((ConsolerMethod)jk.Value.GetCustomAttribute(typeof(ConsolerMethod))) ?? new ConsolerMethod("")).Description ?? ""))
                    var att = (((ConsolerMethod)t.GetCustomAttribute(typeof(ConsolerMethod))) ?? new ConsolerMethod(""));
                    if (!att.IsAlias)
                        strings.Add("   -"+t.Name + (!string.IsNullOrEmpty(att.Description) ? " - "+att.Description : ""));
                }
                Console.WriteLine( helpCommand = string.Join("\n\r", strings));
            }

            [ConsolerMethod(true)]
            public static void Commands() {
                Help();
            }

            [ConsolerMethod(true)]
            public static void methods() {
                Help();
            }


        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ConsolerMethod : Attribute {
        public string Description { get; set; }
        public bool IsAlias { get; set; }
        public ConsolerMethod(bool Alias = false) {
            IsAlias = Alias;
        }
        public ConsolerMethod(string Description) { this.Description = Description; IsAlias = false; }
    }
}