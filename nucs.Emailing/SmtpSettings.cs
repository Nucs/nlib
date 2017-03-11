using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using nucs.Emailing.Helpers;

namespace nucs.Emailing {
    public class SmtpSettings {
        
        public static SmtpSettings LoadConfiguration(string filename = "email.credentials.settings") {
            return LoadConfiguration(new FileInfo(Files.Normalize(filename)));
        }

        public static SmtpSettings LoadConfiguration(FileInfo file) {
            var s = Settings.AppSettings<Configuration>.Load(file.FullName);
            return LoadConfiguration(s);
        }

        public static SmtpSettings LoadConfiguration(Configuration s) {
            return new SmtpSettings() {DefaultSender = s.DefaultSender, EnableSSL = s.EnableSSL, HostIp = s.HostIp, Port = s.Port, UseDefaultCredentials = s.UseDefaultCredentials, Username = s.Username, DefaultSenderDisplayName = s.DefaultSenderDisplayName, Password= Loadpassword(s.Password)};
        }

        /// <summary>
        ///     The default email address the emails will be sent through when not specified.
        ///     e.g.: address@yourdomain.com
        /// </summary>
        public string DefaultSender;

        /// <summary>
        ///     The display name that will be shown - sent from...
        /// </summary>
        public string DefaultSenderDisplayName;

        public string Username;

        public SecureString Password;

        public string HostIp;

        public ushort Port = 587;

        public bool EnableSSL;

        public bool UseDefaultCredentials = true;

        public string PasswordToString() {
            if (Password == null)
                return null;
            IntPtr valuePtr = IntPtr.Zero;
            try {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(Password);
                return Marshal.PtrToStringUni(valuePtr);
            } finally {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private static SecureString Loadpassword(string password) {
            var _ss = new SecureString();
            password.ToCharArray().ToList().ForEach(p => _ss.AppendChar(p));
            return _ss;
        }
    }
}