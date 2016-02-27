using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace nucs.Windows.FileSystem {
    public static class FileHelper {
        private const int RmRebootReasonNone = 0;
        private const int CCH_RM_MAX_APP_NAME = 255;
        private const int CCH_RM_MAX_SVC_NAME = 63;

        /// <summary>
        ///     Gets the directory of the executed file
        /// </summary>
        public static string ExecutedDirectoryPath() {
            var exes = AppDomain.CurrentDomain.GetAssemblies()
                .Select(d => ConfigurationManager.OpenExeConfiguration(d.Location))
                .Where(config => config.HasFile && config.FilePath.Contains(@"vshost.exe") == false)
                
                .ToArray();

            if (exes.Length==0)
                throw new FileNotFoundException();
            if (exes.Length == 1) {
                return Path.GetDirectoryName(exes[0].FilePath);
            }
            throw new Exception("Too many files found to indicate path of exe!");
        }

        public static DirectoryInfo CopyTo(this DirectoryInfo sourceDir, string destinationPath, bool overwrite = false) {
            string sourcePath = sourceDir.FullName;

            var destination = new DirectoryInfo(destinationPath);

            destination.Create();

            foreach (
#if (NET_3_5 || NET_3_0 || NET_2_0)
                string sourceSubDirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
#else
                string sourceSubDirPath in Directory.EnumerateDirectories(sourcePath, "*", SearchOption.AllDirectories))
#endif
                    Directory.CreateDirectory(sourceSubDirPath.Replace(sourcePath, destinationPath));
#if (NET_3_5 || NET_3_0 || NET_2_0)

            foreach (string file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
#else
            foreach (string file in Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories))
#endif
                File.Copy(file, file.Replace(sourcePath, destinationPath), overwrite);

            return destination;
        }

        public static bool OutputStringToFile(this string text, string path) {
            try {
                File.WriteAllText(path, text, Encoding.UTF8);
                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Waits for a process to be free with constant sleep
        /// </summary>
        /// <param name="filename">The path to the file</param>
        /// <param name="delay">The miliseconds to wait</param>
        /// <param name="maxtries">The number of tries, -1 is infinitie</param>
        [DebuggerStepThrough]
        public static bool WaitFileReady(this string filename, int delay=100, int maxtries=-1) {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            while (true) {
                try {
                    using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None)) 
                        if (inputStream.Length > 0) 
                            break;
                    
                } catch (Exception) {}

                if (maxtries != -1) {
                    maxtries--;
                    if (maxtries == 0)
                        return false;
                }
                Thread.Sleep(delay);
            }
            return true;
        }

        /// <summary>
        ///     Searches all directories and file inside the <paramref name="directory"/>.
        /// </summary>
        /// <param name="filename">The filename without extension</param>
        /// <param name="directory">The directory to search, search will be performed to subdirectories</param>
        /// <param name="extension">Possible specification for the extension, for example "exe" or "doc" </param>
        public static string[] SearchFile(string filename, string directory, string extension = null) {
                return Directory.GetFiles(directory, (extension == null ? "" : "*." + extension), SearchOption.AllDirectories)
                        .Where(f => f.EndsWith(filename+"."+extension))
                        .ToArray();
        }


        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        private static extern int RmRegisterResources(uint pSessionHandle,
            UInt32 nFiles,
            string[] rgsFilenames,
            UInt32 nApplications,
            [In] RM_UNIQUE_PROCESS[] rgApplications,
            UInt32 nServices,
            string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        private static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmGetList(uint dwSessionHandle,
            out uint pnProcInfoNeeded,
            ref uint pnProcInfo,
            [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
            ref uint lpdwRebootReasons);

        /// <summary>
        ///     Find out what process(es) have a lock on the specified file.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <returns>Processes locking the file</returns>
        /// <remarks>
        ///     See also:
        ///     http://msdn.microsoft.com/en-us/library/windows/desktop/aa373661(v=vs.85).aspx
        ///     http://wyupdate.googlecode.com/svn-history/r401/trunk/frmFilesInUse.cs (no copyright in code at time of viewing)
        /// </remarks>
        public static List<Process> WhoIsLocking(string path) {
            uint handle;
            string key = Guid.NewGuid().ToString();
            var processes = new List<Process>();

            int res = RmStartSession(out handle, 0, key);
            if (res != 0) throw new Exception("Could not begin restart session.  Unable to determine file locker.");

            try {
                const int ERROR_MORE_DATA = 234;
                uint pnProcInfoNeeded = 0,
                    pnProcInfo = 0,
                    lpdwRebootReasons = RmRebootReasonNone;

                string[] resources = {path}; // Just checking on one resource.

                res = RmRegisterResources(handle, (uint) resources.Length, resources, 0, null, 0, null);

                if (res != 0) throw new Exception("Could not register resource.");

                //Note: there's a race condition here -- the first call to RmGetList() returns
                //      the total number of process. However, when we call RmGetList() again to get
                //      the actual processes this number may have increased.
                res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                if (res == ERROR_MORE_DATA) {
                    // Create an array to store the process results
                    var processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;

                    // Get the list
                    res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                    if (res == 0) {
                        processes = new List<Process>((int) pnProcInfo);

                        // Enumerate all of the results and add them to the 
                        // list to be returned
                        for (int i = 0; i < pnProcInfo; i++) {
                            try {
                                processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                            }
                                // catch the error -- in case the process is no longer running
                            catch (ArgumentException) {
                            }
                        }
                    }
                    else throw new Exception("Could not list processes locking resource.");
                }
                else if (res != 0)
                    throw new Exception("Could not list processes locking resource. Failed to get size of result.");
            }
            finally {
                RmEndSession(handle);
            }

            return processes;
        }

        private enum RM_APP_TYPE {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct RM_PROCESS_INFO {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)] public readonly string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)] public readonly string
                strServiceShortName;

            public readonly RM_APP_TYPE ApplicationType;
            public readonly uint AppStatus;
            public readonly uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)] public readonly bool bRestartable;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RM_UNIQUE_PROCESS {
            public readonly int dwProcessId;
            public readonly FILETIME ProcessStartTime;
        }
    }
}