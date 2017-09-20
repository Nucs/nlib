using System;
using System.Collections.Generic;
using System.Linq;

namespace nucs.Windows.Keyboard.CardReader {
    

    /// <summary>
    /// CR stands for Card Reader (magnetic type).
    /// this class is made for managing a list of keyboards that are represented as CRs.
    /// </summary>
    public static class CRManager {
        private static IDictionary<IntPtr, InputDevice.DeviceInfo> devices = new Dictionary<IntPtr, InputDevice.DeviceInfo>();
        private static List<Device> Devices = new List<Device>();

        public static int Initiallize() {
            devices = InputDevice.GetKeyboardsDictionary();
            foreach (var n in devices) {
                Devices.Add(new Device(n.Value));
            }
            return Devices.Count;
        }

        public static int Refresh() {
            devices.Clear();
            devices = InputDevice.GetKeyboardsDictionary();
            Devices.Clear();
            foreach (var n in devices) {
                Devices.Add(new Device(n.Value));
            }
            return Devices.Count;
        }

        public static bool IsConnected(string val) {
            Refresh();
            return Devices.Any(c => c.name == val) || Devices.Any(c => c.sId == val);
        }

        public static bool IsSelectedConnected(string CRsID) {
            Refresh();
            return Devices.Any(c => c.name == CRsID) || Devices.Any(c => c.sId == CRsID);
        }

        public static List<string> GetNames() {
            Refresh();
            return Devices.Select(d => d.name).ToList();
        }

        public static List<Device> GetList() {
            Refresh();
            return new List<Device>(Devices);
        }

        public static Device Find(string val) {
            Refresh();
            if (Devices.Any(c => c.name == val)) {
                return Devices.FirstOrDefault(c => c.name == val);
            }
            if (Devices.Any(c => c.sId == val)) {
                return Devices.FirstOrDefault(c => c.sId == val);
            }
            return null;
        }

        public class Device {
            public string sId { get; private set; }
            public string name { get; private set; }
            public InputDevice.DeviceInfo info { get; private set; }

            public Device(InputDevice.DeviceInfo info) {
                this.info = info;
                try {
                    this.name = info.Name.Split(';')[1] +":"+ info.deviceHandle;
                } catch {
                    this.name = info.Name + ":" + info.deviceHandle;
                }
                sId = info.deviceName;
            }

            public static implicit operator Device(InputDevice.DeviceInfo di) {
                return new Device(di);
            }

            public override int GetHashCode() {
                return (int)info.deviceHandle;
            }

            public override bool Equals(object obj) {
                return info.deviceHandle == ((InputDevice.DeviceInfo) obj).deviceHandle;
            }

            public override string ToString() {
                return name;
            }
        }
    }
}
