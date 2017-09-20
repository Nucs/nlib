using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using nucs.SystemCore;
using nucs.Windows.FileSystem;

namespace nucs.Logger {
    public static class Logy {
        public delegate string PrefixGeneratorHandle();

        public static PrefixGeneratorHandle PrefixGenerator;

        static Logy() {
            PrefixGenerator += () => $"[{DateTime.Now.ToString("F", CultureInfo.InvariantCulture)}]";
        }

        public static string Base = FileHelper.ExecutedDirectoryPath() + "/logs/";

        public static string Filename = DateTime.Now.ToString("yy-MM-dd");
        public static string Extension = "log";

        public static object FileWriteLocker = new object();
        
        public static void Log(string txt) {
            LogFile.Directory.EnsureDirectoryExists();
            lock (FileWriteLocker) {
                _retry:
                try {
                    using (var stream = LogFile.AppendText()) {
                        stream.Write(PrefixGenerator() + txt + Environment.NewLine);
                    }
                }
                catch (Exception) { 
                    Thread.Sleep(10);
                    goto _retry;
                }
            }
        }

        public static void Log(params object[] txt) {
            Log(txt.StringJoin(""));
        }


        public static FileInfo LogFile {
            get {
                return new FileInfo(Base+Filename+"."+Extension);
            }
        }




    }
}
