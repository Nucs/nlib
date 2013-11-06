using System;
using System.Diagnostics;
using System.Linq;

namespace nucs.Windows.Processes {
    public static class ProcessFinder {
        // Returns the name of the process owning the foreground window.
        public static Process GetForegroundProcess() {
            var hwnd = NativeWin32.GetForegroundWindow();

            // The foreground window can be NULL in certain circumstances, 
            // such as when a window is losing activation.
            if (hwnd == null || hwnd == IntPtr.Zero)
                return null;

            uint pid;
            NativeWin32.GetWindowThreadProcessId(hwnd, out pid);

            return Process.GetProcesses().FirstOrDefault(p => p.Id == pid);
        }

        public static string GetForegroundProcessName() {
            return GetForegroundProcess().ProcessName;
        }
        


        public static bool ProcessExists(ProcessInfo process) {
            try {
                var procs = Process.GetProcesses();
                return procs.Any(process.Equals);
            } catch { return false; }
        }

        /// <summary>
        /// Iterates through the open processes list and find if any of them is named by <paramref name="processName"/>
        /// </summary>
        /// <param name="processName">The name of the process as can be found at Window's Task Manager</param>
        /// <returns>If exists</returns>
        public static bool ProcessExists(string processName) {
            try {
                var procs = Process.GetProcessesByName(processName);
                return procs.Length > 0;
            } catch { return false; }
        }

        public static Process FindProcess(ProcessInfo info) {
            return
                Process.GetProcesses().FirstOrDefault(
                    p => p.Id == info.UniqueID && p.MachineName == info.MachineName && p.ProcessName == info.Name);
        }

        public static bool ProcessExists(Process process) {
            try {
                return Process.GetProcesses().Any(p =>
                     {
                         try {
                             if (p.Id == process.Id && p.ProcessName == process.ProcessName && p.MachineName == process.MachineName) return true;
                             return false;
                         }
                         catch {
                             return false;
                         }
                                                  });
            }
            catch { return false; }
        }

        public static ProcessInfo ToProcessInfo(this Process proc) {
            return new ProcessInfo(proc);
        }

    }
}
