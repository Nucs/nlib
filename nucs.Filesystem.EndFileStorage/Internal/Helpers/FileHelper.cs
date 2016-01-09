using System;
using System.IO;

namespace nucs.EndData.Helpers {
    internal static class FileHelper {

        public static bool IsFileLocked(this string file) { return IsFileLocked(new FileInfo(file));}
        public static bool IsFileLocked(this FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                stream?.Close();
            }

            //file is not locked
            return false;
        }

        public static bool WaitWhileLocked(this string file, int timeout = -1) { return WaitWhileLocked(new FileInfo(file), -1);}

        public static bool WaitWhileLocked(this FileInfo file, int timeout = -1) {
            if (!file.IsFileLocked())
                return true;
            var starttime = DateTime.Now;
            var ts = TimeSpan.FromMilliseconds(timeout);
            bool result = false;
            while (!(result = !IsFileLocked(file)) && timeout == -1 || DateTime.Now-starttime > ts)
                System.Threading.Thread.Sleep(50);
            return result;
        }

    }
}