using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TFer.Filesystem.Enumerators {
    public class ActiveProcessFiles : IEnumerable<FileInfo> {
        public IEnumerator<FileInfo> GetEnumerator() {
            var p = Process.GetProcesses();
            foreach (var proc in p) {
                FileInfo _r;
                try {
                    _r = new FileInfo(proc.Modules[0].FileName);
                } catch { continue; }
                yield return _r;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}