using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TFer.Modules.DllLoader.Compression;
using TFer.Modules.DllLoader.Resource;

namespace TFer.Modules.DllLoader.Injection {

    // ReSharper disable once InconsistentNaming
    public static class CLRInjector {
        public static void Inject(Process proc, FileInfo injectable, string arguments="") {
            //export libs
            var tempdir = getTemporaryDirectory();
            ZipResource.Export(tempdir, "ibs"+(Architecture.Is64BitOperatingSystem ? "64":"86"), "0x7355396FF044F879ADE3C007F54412F810741036");

            // build args
            var item = extractInjectableMethods(injectable.FullName).FirstOrDefault(m => m.Name == "_main");
            if (item==null)
                throw new InvalidOperationException("The given injectable is not injectable");

            var args = $"-m {item.Name} -i \"{injectable.FullName}\" -l {item.TypeName} -a \"{arguments}\" -n {proc.Id}";

            // create the process info
            var info = new ProcessStartInfo {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = Path.Combine(tempdir, "Inject.exe"),
                Arguments = args
            };
            var injector = Process.Start(info);
            injector.WaitForExit();
        }

        private static string getTemporaryDirectory() {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private static IEnumerable<MethodItem> extractInjectableMethods(string file) {
            // open assembly
            Assembly asm = Assembly.LoadFile(file);

            // get valid methods
            return
                (from c in asm.GetTypes()
                    from m in c.GetMethods(BindingFlags.Static |
                                           BindingFlags.Public | BindingFlags.NonPublic)
                    where m.ReturnType == typeof (int) &&
                          m.GetParameters().Length == 1 &&
                          m.GetParameters().First().ParameterType == typeof (string)
                    select new MethodItem {
                        Name = m.Name,
                        ParameterName = m.GetParameters().First().Name,
                        TypeName = m.ReflectedType.FullName
                    });

            // ...
        } 
    }
}