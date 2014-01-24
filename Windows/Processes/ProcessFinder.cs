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


        /// <summary>
        ///     Iterates through the open processes list and finds the procs based on the given parameters.
        /// </summary>
        /// <param name="method">The different methods you can use to find a process.</param>
        /// <param name="value">The value that must fit into the method's object that can be found in the doc.</param>
        /// <returns>The processes in array or an empty array</returns>
        /// <exception cref="ArgumentException">Incorrect type of <see cref="object"/> was passed.</exception>
        public static Process[] Find(ProcessSearchMethod method, object value) {
            var procs = new Process[0];
            switch (method) {
                case ProcessSearchMethod.TitleName:
                    if (value is string == false) throw new ArgumentException("Incorrect value was passed compared to the method (" + method + ")");
                    return Process.GetProcesses().Where(p => p.MainWindowTitle.ToLowerInvariant().Contains((value as string).ToLowerInvariant())).ToArray();
                case ProcessSearchMethod.ProcessName:
                    if (value is string == false) throw new ArgumentException("Incorrect value was passed compared to the method (" + method + ")");
                    var s = value as string;
                    return Process.GetProcesses().Where(p => p.ProcessName.Contains(s.Contains(".") ? s.Split('.')[0] : s)).ToArray();
                case ProcessSearchMethod.Handle:
                    if (value is IntPtr == false) throw new ArgumentException("Incorrect value was passed compared to the method ("+method+")");
                    var ptr = (IntPtr) value;
                    return Process.GetProcesses().Where(p => p.Handle.Equals(ptr)).ToArray();
                case ProcessSearchMethod.ProcessInfo:
                    if (value is ProcessInfo == false) throw new ArgumentException("Incorrect value was passed compared to the method (" + method + ")");
                    var pi = value as ProcessInfo;
                    var proc = pi.ToProcess();
                    return proc == null ? new Process[0] : new[] {proc};
                case ProcessSearchMethod.Process:
                    if (value is Process == false) throw new ArgumentException("Incorrect value was passed compared to the method ("+method+")");
                    var process = (Process)value;
                    return Process.GetProcesses().Where(p => p.Id == process.Id && p.ProcessName == process.ProcessName && p.MachineName == process.MachineName).ToArray(); 

                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }

        /// <summary>
        ///     Iterates through the list of processes and if any fit the parameters, it returns true.
        /// </summary>
        /// <param name="method">The different methods you can use to find a process.</param>
        /// <param name="value">The value that must fit into the method's object that can be found in the doc.</param>
        /// <returns>The process exists</returns>
        /// <exception cref="ArgumentException">Incorrect type of <see cref="object"/> was passed.</exception>
        public static int Exists(ProcessSearchMethod method, object value) {
            return Find(method, value).Length;
        }

        /// <summary>
        /// Converts the process into a <see cref="ProcessInfo"/> object.
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        public static ProcessInfo ToProcessInfo(this Process proc) { return new ProcessInfo(proc); }
    }
    public enum ProcessSearchMethod {
        /// <summary>
        ///     The title of the process
        /// </summary>
        TitleName,
        /// <summary>
        ///     The process name that can be observed at the task manager 'Processes' tab.
        /// </summary>
        ProcessName,
        /// <summary>
        ///     The handle of the process.
        /// </summary>
        Handle,
        /// <summary>
        ///     The <see cref="ProcessInfo"/> object
        /// </summary>
        ProcessInfo,
        /// <summary>
        ///     Finds the process because the object might refer to a dead object. use this to check or find if it is still alive.
        /// </summary>
        Process
    }
}