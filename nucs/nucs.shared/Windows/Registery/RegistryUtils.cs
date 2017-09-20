#if !NETSTANDARD2_0

using Microsoft.Win32;
using nucs.Windows;

namespace nucs.Windows.Registry {
    public class RegistryUtils {
        private static RegistryKey OpenKey(string key, bool writeable, out string kval) {
            var ios = key.LastIndexOfAny(new[] {'\\'});
            var kkey = key.Substring(0, ios);
            kval = key.Substring(ios + 1, key.Length - ios - 1);
            RegistryKey baseReg;

            if (key.StartsWith("HKLM")) {
                baseReg = Microsoft.Win32.Registry.LocalMachine;
                kkey = kkey.Substring(5);
            } else if (key.StartsWith("HKCU")) {
                baseReg = Microsoft.Win32.Registry.CurrentUser;
                kkey = kkey.Substring(5);
            } else {
                baseReg = SystemInfo.IsAdministrator
                    ? Microsoft.Win32.Registry.LocalMachine
                    : Microsoft.Win32.Registry.CurrentUser;
            }

            return baseReg.OpenSubKey(kkey, writeable);
        }

        /// <summary>
        ///     Usage: RegistryUtils.Write(@"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProductId", 123);
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Write(string key, object value) {
            try {
                string kval;
                using (var regKey = OpenKey(key, true, out kval)) {
                    regKey.SetValue(kval, value);
                }
            } catch {}
        }
        /// <summary>
        /// Usage RegistryUtils.Read(@"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProductId");
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Read(string key) {
            string kval;
            using (var regKey = OpenKey(key, false, out kval)) {
                return "" + regKey.GetValue(kval);
            }
        }
    }
}

#endif