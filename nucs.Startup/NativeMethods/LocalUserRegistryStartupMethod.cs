using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using nucs.Filesystem;
using nucs.Startup.Internal;

namespace nucs.Startup.NativeMethods {
    public class LocalUserRegistryStartupMethod  : IStartupMethod {
        public uint Priority => 2;

        public string DefaultArguments = "";

        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public bool Attach(FileCall file, string alias = null) {
            if (!IsAttachable) //check attachablility
                throw new InvalidOperationException("Unable to attach using this method. avoid calling before checking.");

            var alias_resolve = (string.IsNullOrEmpty(alias)
                    ? file.GetFileNameWithoutExtension()
                    : alias);
            if (Attached.Any(attfile => attfile.FullName.Equals(file.FullName))) //check if already set to startup.
                throw new InvalidOperationException("This file is already attached.");
            using (RegistryKey add = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true)) {
                if (add == null)
                    throw new Exception("Invalid registery path/not found.");
                
                try {
                    add.SetValue(alias_resolve, "\"" + file.FullName + "\"" + (string.IsNullOrEmpty(DefaultArguments) ? "" : (" " + DefaultArguments)));
                } catch {
                    return false;
                }
                return true;
            }
        }

        public bool Disattach(FileInfo fc) {
            using (RegistryKey add = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true)) {
                if (add == null)
                    throw new Exception("Invalid registery path/not found. - This method is unavailable.");

                var result = add.GetValueNames()
                    .Select(name => new {Name=name, Value=add.GetValue(name).ToString()})
                    .FirstOrDefault(kv=>kv.Value.Contains(fc.FullName));
                if (result == null) return false;

                try {
                    add.DeleteValue(result.Name);
                    return true;
                } catch {
                    return false;
                }

            }
        }

        public bool IsAttached(string alias) {
            using (RegistryKey add = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true)) {
                if (add == null)
                    throw new Exception("Invalid registery path/not found. - This method is unavailable.");

                return add.GetValueNames().Any(vn => vn.Equals(alias));
            }
        }

        public bool IsAttached(FileInfo file) {
            return Attached.Any(fi => fi.FullName.Equals(file.FullName));
        }

        public bool IsAttachable {
            get {
                var alias = "potatoprog" + StringGenerator.Generate(5);
                var file = new FileInfo("C:/" + alias + ".exe");
                try {
                    using (RegistryKey add = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true)) {
                        if (add == null)
                            return false;
                        add.SetValue(alias, "\"" + file.FullName + "\"");
                        add.DeleteValue(alias);
                
                        return true;
                    }
                } catch {
                    return false;
                }
            }
        }

        public IEnumerable<FileCall> Attached {
            get {
                using (RegistryKey add = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false)) {
                    if (add == null)
                        throw new Exception("Invalid registery path/not found. - This method is unavailable.");

                    var paths = add.GetValueNames().Select(name => new {alias=name, path=add.GetValue(name).ToString()});
                    foreach (var filecall in paths.Where(filecall=>!string.IsNullOrEmpty(filecall.path))) {
                        yield return new FileCall(filecall.path, filecall.alias);
                    }
                }
            }
        }
    }
}