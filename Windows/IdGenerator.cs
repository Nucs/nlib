using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace nucs.Windows {
    public static class IdGenerator {
        private static Guid _pcid = Guid.Empty;
        /// <summary>
        ///     Takes the Volume serial of the windows's root drive, hashes it using SHA1 and generates from it a Guid.
        /// </summary>
        /// <returns></returns>
        public static Guid GetPCUniqueId() {
            if (_pcid != Guid.Empty)
                return _pcid;
            var drive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)).Trim(':','\\');
            var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            try {
                dsk.Get();
            } catch (ManagementException e) {
                if (e.Message.Contains("Not found"))
                    return Guid.Empty;
                throw e;
            }
            var serial = dsk["VolumeSerialNumber"].ToString();
            using (SHA1 sha = SHA1.Create()) {
                byte[] hash = sha.ComputeHash(Encoding.Default.GetBytes(serial));
                return _pcid = new Guid(hash.Take(16).ToArray());
            }


        }
    }
}
