using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using TFer.Modules.DllLoader.Injection;

namespace TFer.Modules.DllLoader {
    class Program {

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        static int Main(string[] args) {
            _main(args==null || args.Length==0 ? "" : args[0]);
            return 1;
        }
        public static FileInfo ExecutingExe => new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);

        public static int _main(string _arg) {
            var args = ArgsParser.Parse(_arg);
            
#if DEBUG
            AllocConsole();
#endif

            switch (args["a"]) {
                case "inject":
                    if (File.Exists(args["toinj"])) {
                        CLRInjector.Inject(
                            Process.GetProcessById(Convert.ToInt32(args["procid"]))
                            , new FileInfo(args["toinj"]),
                            args["injargs"]
                            );
                    }
                    break;
                case "load":
                    new DllLoader(new FileInfo(args["toload"]), args["toloadargs"]?.Split(new[] {"%%%"}, StringSplitOptions.RemoveEmptyEntries));
                    break;
                case "injected":
                    while (true) {
                        Thread.Sleep(1000);
                        Console.Write(".");
                    }

            }
            return 1;
        }
    }
}
