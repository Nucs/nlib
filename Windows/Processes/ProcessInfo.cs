using System.Diagnostics;
using nucs.SystemCore.String;

namespace nucs.Windows.Processes {
    public class ProcessInfo {
        public int UniqueID { get; private set; }
        public string Name { get; private set; }
        public string MachineName { get; private set; }

        public ProcessInfo(Process proc) {
            if (proc == null)
                return;
            UniqueID = proc.Id;
            Name = proc.ProcessName;
            MachineName = proc.MachineName;
        }

        public ProcessInfo(int UniqueID, string Name, string MachineName) {
            this.UniqueID = UniqueID;
            this.Name = Name;
            this.MachineName = MachineName;
        }

        public bool Equals(Process proc) {
            try {
                return (UniqueID == proc.Id && Name == proc.ProcessName && MachineName == proc.MachineName);
            } catch {
                return false;
            }
        }

        public bool Equals(ProcessInfo proc) {
            try {
                return (UniqueID == proc.UniqueID && Name == proc.Name && MachineName == proc.MachineName);
            } catch {
                return false;
            }
        }

        public bool Exists() {
            return ProcessFinder.ProcessExists(this);
        }

        public Process ToProcess() {
            return ProcessFinder.FindProcess(this);
        }

        public bool WaitForExit(uint timeout = 0) {
            try {
                var proc = this.ToProcess();
                if (proc == null) return true;
                if (timeout == 0) {
                    proc.WaitForExit();
                    return true;
                }
                return proc.WaitForExit((int) timeout);
            } catch {
                return true;
            }
        }

        public override string ToString() {
            return Name + "↔" + UniqueID + "↔" + MachineName;
        }

        public static ProcessInfo TryParse(string toString) {
            try {
                var s = toString.Split('↔');
                return new ProcessInfo(s[1].ToInt32(), s[0], s[2]);

            }
            catch {
                return null;
            }
        }

    }
}
