using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TFer.Modules.TestModule {
    class Program {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        static int Main(string[] args) {
            _main(args==null || args.Length==0 ? "" : args[0]);
            return 1;
        }

        public static FileInfo ExecutingExe => new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);

        public static int _main(string _arg) {
            AllocConsole();
            var tv = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == "TeamViewer");
           // InjectWrapper.Inject(tv, new FileInfo("C:/injectable.dll"), "wow");
            //Console.WriteLine(_arg);
            //var args = ArgsParser.Parse(_arg);
            //var @out = string.Join(",", args.Select(kv => $"{kv.Key} => {kv.Value}").ToArray());
            //MessageBox.Show($"Args: {_arg}\nParsed: {_arg}");
            return 1;
        }
    }
}
