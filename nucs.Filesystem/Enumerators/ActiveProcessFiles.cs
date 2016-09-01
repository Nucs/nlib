using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace nucs.Filesystem.Enumerators {
    public class ActiveProcessFiles : IEnumerable<FileInfo> {
        public IEnumerator<FileInfo> GetEnumerator() {
            var p = Process.GetProcesses();
            foreach (var proc in p) {
                FileInfo _r;
                try {
                    var path = ProcessExecutablePath(proc);
                    if (string.IsNullOrEmpty(path))
                        continue;
                    _r = new FileInfo(path);
                } catch { continue; }
                yield return _r;
            }
        }

        /// <summary>
        ///     Will enumerate through all FileInfos and return the matching one to the comperator.
        /// </summary>
        /// <param name="comperator"></param>
        /// <returns></returns>
        public Process Enumerate(Func<FileInfo, bool> comperator) {
            var p = Process.GetProcesses();
            foreach (var proc in p) {
                FileInfo _r;
                try {
                    var path = ProcessExecutablePath(proc);
                    if (string.IsNullOrEmpty(path))
                        continue;
                    _r = new FileInfo(path);
                } catch { continue; }
                if (comperator(_r))
                    return proc;
            }
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        private static string ProcessExecutablePath(Process process)
        {
            try
            {
                return process.MainModule.FileName;
            }
            catch
            {
                var query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                var searcher = new ManagementObjectSearcher(query);

                foreach (var o in searcher.Get())
                {
                    var item = (ManagementObject)o;
                    var id = item["ProcessID"];
                    var path = item["ExecutablePath"];

                    if (path != null && id.ToString() == process.Id.ToString())
                    {
                        return path.ToString();
                    }
                }
            }

            return null;
        }
    }
}