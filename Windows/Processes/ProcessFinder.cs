using System;
using System.Diagnostics;
using System.Linq;

namespace nucs.Windows.Processes {
    public static class ProcessFinder {
        // Returns the name of the process owning the foreground window.
        public static Process GetForegroundProcess() {
            IntPtr hwnd = NativeWin32.GetForegroundWindow();

            // The foreground window can be NULL in certain circumstances, 
            // such as when a window is losing activation.
            if (hwnd == null || hwnd == IntPtr.Zero)
                return null;

            uint pid;
            NativeWin32.GetWindowThreadProcessId(hwnd, out pid);

            return Process.GetProcesses().FirstOrDefault(p => p.Id == pid);
        }

        public static string GetForegroundProcessName() { return GetForegroundProcess().ProcessName; }


        public static bool ProcessExists(ProcessInfo process) {
            try {
                Process[] procs = Process.GetProcesses();
                return procs.Any(process.Equals);
            } catch {
                return false;
            }
        }

        /// <summary>
        ///     Iterates through the open processes list and find if any of them is named by <paramref name="processName" />
        /// </summary>
        /// <param name="name">The name of the process as can be found at Window's Task Manager (Not in process tab)</param>
        /// <returns>If exists</returns>
        public static bool ExistsByName(string name) {
            try {
                Process[] procs = Process.GetProcessesByName(name);
                return procs.Length > 0;
            } catch {
                return false;
            }
        }

        /// <summary>
        ///     Iterates through the open processes list and find if any of them is named by <paramref name="processName" />
        /// </summary>
        /// <param name="processName">The name of the process as can be found at Window's Task Manager</param>
        /// <returns>If exists</returns>
        public static bool ExistsByProcName(string processName) {
            return Process.GetProcesses().Any(p => p.ProcessName.Contains(processName.Contains(".") ? processName.Split('.')[0] : processName));
        }

        /// <summary>
        ///     Finds the process based on the info, if not found null is returned.
        /// </summary>
        public static Process FindProcess(ProcessInfo info) { return Process.GetProcesses().FirstOrDefault(p => p.Id == info.UniqueID && p.MachineName == info.MachineName && p.ProcessName == info.Name); }

        public static bool ProcessExists(Process process) {
            try {
                return Process.GetProcesses().Any(p => {
                                                      try {
                                                          if (p.Id == process.Id && p.ProcessName == process.ProcessName && p.MachineName == process.MachineName) 
                                                              return true;
                                                          return false;
                                                      } catch {
                                                          return false;
                                                      }
                                                  });
            } catch {
                return false;
            }
        }

        public static ProcessInfo ToProcessInfo(this Process proc) { return new ProcessInfo(proc); }
    }
}