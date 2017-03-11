using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using nucs.Emailing.Helpers;

namespace nucs.Emailing {
    public class SimpleLog {
        public static SimpleLog LoadConfiguration(string filename = "email.credentials.settings") {
            return LoadConfiguration(new FileInfo(Files.Normalize(filename)));
        }

        public static SimpleLog LoadConfiguration(FileInfo file) {
            var s = Settings.AppSettings<Configuration>.Load(file.FullName);
            return LoadConfiguration(s);
        }

        public static SimpleLog LoadConfiguration(Configuration s) {
            return new SimpleLog() {LogDirectoryName = s.LogDirectoryName, LogLocally = s.LogLocally};
        }

        /// <summary>
        ///     Log all emails sent to a local directory specified in <paramref name="LogDirectoryName"/>
        /// </summary>
        public bool LogLocally = false;

        /// <summary>
        ///     The directory name for logging, plain word.
        ///     e.g.: log
        /// </summary>
        public string LogDirectoryName = "log";

        /// <summary>
        ///     The exe that has started this process
        /// </summary>
        private static FileInfo ExecutingExe {
            get {
                var fn = Assembly.GetEntryAssembly().Location;
                if (fn != null)
                    return new FileInfo(fn);
                return new FileInfo("");
            }
        }

        /// <summary>
        ///     The directory that the executing exe is inside
        /// </summary>
        private static DirectoryInfo ExecutingDirectory => ExecutingExe.Directory;

        public void LogMessage(string subject, string[] tos, string from, string sender, string body) {
            foreach (var to in tos) {
                
            var @out = Path.Combine(LoggingDirectory.FullName, $"{DateTime.Now.Ticks}.{CleanForFileName(subject)}.email.txt");
            var cont =
                $@"At: {DateTime.Now:s}
To: {to}
From: {from}
Sender: {sender}
Title: {subject}

{body}";
                //try {
                    File.WriteAllText(@out, cont);
                /*}
                catch (Exception e) {
                    
                }*/
            }
        }


        private static string CleanForFileName(string fileName) {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }


        private readonly object logdir_sync = new object();
        private DirectoryInfo _logdir;

        public DirectoryInfo LoggingDirectory {
            get {
                lock (logdir_sync) {
                    if (_logdir == null) {
                        _logdir = new DirectoryInfo(Path.Combine(ExecutingDirectory.FullName, LogDirectoryName));
                        if (!Directory.Exists(_logdir.FullName))
                            _logdir.Create();
                    }
                    return _logdir;
                }
            }
        }
    }
}