using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
#if !AV_SAFE
using nucs.Windows.Startup;
#endif
namespace nucs.Windows.Security {
    public static class SecurityDiagnoser {
        #if !AV_SAFE


        /// <summary>
        /// Tests access for 3 types of startup and returns the most global startup option that is allowed. See 'StartupType'
        /// </summary>
        /// <returns></returns>
        public static StartupType GetPreferedAllowedStartupType() {
            return StartupManager.GetPreferedAllowedStartupType();
        }

        /// <summary>
        /// Tests access for 3 types of startup and returns the most global startup option that is allowed. See 'StartupType'
        /// </summary>
        /// <returns></returns>
        public static StartupType GetPreferedAllowedStartupType(string pathToFile) {
            return StartupManager.GetPreferedAllowedStartupType(pathToFile);
        }
#endif
        /// <summary>
        /// Tests all partitions for writing access, reading access and deleting and returns the partition for e.g. "C:\\". returns null if none was found.
        /// </summary>
        /// <returns></returns>
        public static string GetAllowedPartitionForRWD(IEnumerable<char> otherThen) {
            foreach (var strDrive in Environment.GetLogicalDrives().Where(s=>otherThen.Any(j=> j == s[0]) == false)) {
                var path = strDrive + "ATI\\Tools\\";
                try {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(path + "ttt\\");
                    new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.None, path).Demand();
                    new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.View, path).Demand();
                    new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.Change, path).Demand();
                    File.WriteAllText(path + "testicales.exe", "abc123אדש", Encoding.BigEndianUnicode);
                    if (!File.Exists(path + "testicales.exe"))
                        continue;
                    new FileIOPermission(FileIOPermissionAccess.Write, AccessControlActions.Change, path).Demand();
                    File.WriteAllText(path + "testicales.exe", "abc123אדש", Encoding.BigEndianUnicode);
                    new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.None, path + "testicales.exe").Demand();
                    new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.View, path +  "testicales.exe").Demand();
                    new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.Change, path + "testicales.exe").Demand();
                    File.Delete(path + "testicales.exe");
                    Directory.Delete(path+"ttt\\");
                    return strDrive;
                } catch {}
            }
            return null;
        }

        /// <summary>
        /// gets the first partition to allow Reading, writing, deleting. tests folder for e.g. "C:\\ATI\\POTATO\\testfile.exe"
        /// </summary>
        /// <returns></returns>
        public static string GetAllowedPartitionForRWD() { //todo improve to explained return whats allowed and what is not.
            return GetAllowedPartitionForRWD(new char[0]);
        }

        /// <summary>
        /// Tests the given path to ensure you can read, write and delete there. if any of it fails, returns false.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPathAllowedRWD(string path) {
            if (path.Last() != '\\')
                path += "\\";
            try {
                Directory.CreateDirectory(path+"lol321\\");
                new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.None, path).Demand();
                new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.View, path).Demand();
                new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.Change, path).Demand();
                File.WriteAllText(path + "\\" + "testicales.exe", "abc123אדש", Encoding.BigEndianUnicode);
                if (!File.Exists(path + "\\" + "testicales.exe"))
                    return false;
                new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.None, path + "\\" + "testicales.exe").Demand();
                new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.View, path + "\\" + "testicales.exe").Demand();
                new FileIOPermission(FileIOPermissionAccess.AllAccess, AccessControlActions.Change, path + "\\" + "testicales.exe").Demand();
                File.Delete(path + "\\" + "testicales.exe");
                Directory.Delete(path + "lol321\\");
                return true;
            } catch { return false; }
        }
    }

    public class SecurityCheckFailed : Exception {
        public SecurityCheckFailed(string Message) : base(Message) { }
        public SecurityCheckFailed(string Message, Exception innerException) : base(Message, innerException) { }
        public SecurityCheckFailed() : base() { }
    }
}
