/*using System;
using System.Diagnostics;
using System.IO;
using TFer.Filesystem.AI;

namespace EndDataIO {
    public static class EndDataUpdaterWrapper {

        public static void WriteEndData(FileInfo file, byte[] data, int timeout = -1, bool deleteupdater = true) {
            var resource =
#if DEBUG
                ResourceHelper.GetResource("EndDataUpdater.D.exe");
#else
                ResourceHelper.GetResource("EndDataUpdater.R.exe");
#endif
            FileInfo updater;
            _reattempt:
            try {
                updater = Paths.RandomLocation;
                var fs = new FileStream(updater.FullName, FileMode.OpenOrCreate);
                fs.SetLength(0);
                var resdata = resource.ReadAll();
                fs.Write(resdata, 0, resdata.Length);
                updater.Attributes = FileAttributes.Hidden | FileAttributes.System;
            } catch {
                goto _reattempt; //location not writable or of some sort.
            }


            var args = $"\"{file.FullName}\" {Convert.ToBase64String(data)} {timeout} {deleteupdater}";


            var pi = new ProcessStartInfo(updater.FullName, args);

            Process.Start(pi);
        }
    }
}*/