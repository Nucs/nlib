using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;

namespace TFer.Filesystem.Monitoring {
    public class ProcessRestartMonitor {
        public static Thread WerFaultKiller = new Thread(WerFaultHunter);
        private string lasttitle = "";

        static ProcessRestartMonitor() {
            WerFaultKiller.Start();
        }

        public ProcessRestartMonitor(int id) {
            if (id <= 0) throw new ArgumentException(nameof(id));
            var proc = Process.GetProcesses().FirstOrDefault(p => p.Id == id);
            if (proc == null) throw new NullReferenceException("The given proc ID is not found on any open process.");

            _load(proc);
        }

        public ProcessRestartMonitor(Process proc) {
            _load(proc);
        }

        /// <summary>
        ///     Has Stop() been called, if thread has already stopped it will be true.
        /// </summary>
        public bool IsStopping { get; private set; }

        /// <summary>
        ///     Once stopped and thread is dead, its true
        /// </summary>
        public bool HasStopped => IsStopping && !Thread.IsAlive;

        /// <summary>
        ///     File taken from the process
        /// </summary>
        public FileInfo File { get; private set; }

        /// <summary>
        ///     Id that changes with every rebind.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Attempts to find the current process that is monitored, null if faultly not available atm.
        /// </summary>
        public Process RetrieveProcess
        {
            get
            {
                try {
                    return Process.GetProcessById(Id);
                } catch {
                    return null;
                }
            }
        }

        /// <summary>
        ///     Name of the active process.
        /// </summary>
        public string Name { get; private set; }

        public string Title
        {
            get
            {
                var p = RetrieveProcess;
                var tit = p.MainModule.FileVersionInfo.FileDescription;
                if (tit == "")
                    tit = p.ProcessName;
                lasttitle = tit;
                return lasttitle;
            }
        }

        /// <summary>
        ///     Active monitoring thread.
        /// </summary>
        public Thread Thread { get; set; }

        private static void WerFaultHunter(object o) {
            while (true) {
                Thread.Sleep(1500);
                var l = Process.GetProcesses().Where(pp => pp.ProcessName.Contains("WerFault")).ToList();
                if (l.Count > 0) {
                    Thread.Sleep(4000);
                    Process.GetProcesses().Where(pp => pp.ProcessName.Contains("WerFault")).ToList().ForEach(p => p.Kill());
                }
            }
        }

        /// <summary>
        ///     Called after the process has been restarted.
        /// </summary>
        public event Action<Process> ProcessRestarted;

        private void _load(Process proc) {
            File = new FileInfo(ProcessExecutablePath(proc));
            Name = proc.ProcessName;
            Id = proc.Id;
            Thread = new Thread(Monitor);
            Thread.Start(this);
        }

        /// <summary>
        ///     Signals the thread to stop.
        /// </summary>
        public void Stop() {
            IsStopping = true;
        }

        private void Monitor(object o) {
            var parent = o as ProcessRestartMonitor;
            var proc = RetrieveProcess;
            while (true) {
                if (IsStopping)
                    break;
                _rewait:
                if (!proc.WaitForExit(100)) {
                    if (IsStopping)
                        break;
                    goto _rewait;
                }
                if (IsStopping)
                    break;
                proc = Process.Start(File.FullName);
                Name = proc.ProcessName;
                Id = proc.Id;
                ProcessRestarted?.Invoke(proc);
            }
        }

        public static string ProcessExecutablePath(Process process) {
            try {
                return process.MainModule.FileName;
            } catch {
                var query = "SELECT ExecutablePath, ProcessID FROM Win32_Process";
                var searcher = new ManagementObjectSearcher(query);

                foreach (var o in searcher.Get()) {
                    var item = (ManagementObject) o;
                    var id = item["ProcessID"];
                    var path = item["ExecutablePath"];

                    if (path != null && id.ToString() == process.Id.ToString()) {
                        return path.ToString();
                    }
                }
            }

            return "";
        }
    }
}