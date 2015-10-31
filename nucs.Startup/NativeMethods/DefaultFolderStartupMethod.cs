using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using IWshRuntimeLibrary;
using nucs.Filesystem;
using File = System.IO.File;

namespace nucs.Startup.NativeMethods {
    public class DefaultFolderStartupMethod : IStartupMethod {
        public uint Priority => 3;

        private readonly DirectoryInfo StartupFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
        public bool Attach(FileCall file, string alias = null) {
            if (!IsAttachable) 
                throw new InvalidOperationException("Unable to attach using this method. avoid calling before checking.");
            if (Attached.Any(attfile => attfile.FullName.Equals(file.FullName))) //check if already set to startup.
                throw new InvalidOperationException("This file is already attached.");
            var link = (IShellLink) new ShellLink();
            link.SetDescription("My Description");
            link.SetPath(file.FullName);
            
            string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                                                ((string.IsNullOrEmpty(alias)
                                                    ? file.GetFileNameWithoutExtension()
                                                    : alias) + ".lnk"));
            if (File.Exists(targetPath)) //validate file doesnt exist before saving.
                throw new InvalidOperationException("The name of the supposed shortcut is already taken for another file. change the alias name");

            ((IPersistFile)link).Save(targetPath, false);

            return File.Exists(targetPath);
        }

        public bool Disattach(FileInfo fc) {
            var files = StartupFolder.GetFiles();
            IWshShell shell = new WshShell();
            foreach (var shortcut in files.Where(f=>f.Extension.Contains("lnk"))) {
                IWshShortcut p;
                try {
                    p = shell.CreateShortcut(shortcut.FullName) as IWshShortcut;
                } catch (COMException) {
                    continue;
                }
                
                if (p == null) continue;
                FileInfo @out;
                try {
                    @out = new FileInfo(p.TargetPath);
                } catch {
                    continue;
                }
                if (p.TargetPath.Equals(fc.FullName)) {
                    try {
                        shortcut.Delete();
                    } catch {
                        return false;
                    }
                    return true;
                }
            }
            return true;
        }

        public bool IsAttached(string alias) {
            return StartupFolder.GetFiles().Select(f => Path.GetFileNameWithoutExtension(f.FullName)).Any(f => f.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsAttached(FileInfo file) {
            return Attached.Any(t => t.FullName.Equals(file.FullName));
        }

        public bool IsAttachable => StartupFolder.IsDirectoryWritable();

        public IEnumerable<FileCall> Attached {
            get {
                var files = StartupFolder.GetFiles();
                IWshShell shell = new WshShell();
                foreach (var shortcut in files) {
                    IWshShortcut p;
                    try {
                        p = shell.CreateShortcut(shortcut.FullName) as IWshShortcut;
                    } catch (COMException) {
                        continue;
                    }
                    if (p == null) continue;
                    FileInfo @out;
                    try {
                        @out = new FileInfo(p.TargetPath);
                    } catch {
                        continue;
                    }
                    yield return new FileCall(@out, p.Arguments, Path.GetFileNameWithoutExtension(shortcut.FullName));
                }
            }
        }
    }

}

/*
 
                case StartupType.DefaultStartupFolder:
                    if (File.Exists(path) == false)
                        throw new IOException("Could not find file for shortcut creation at/in \"" + path + "\"");
                    var link = (IShellLink) new ShellLink();
                    link.SetDescription("My Description");
                    link.SetPath(path);
                    string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                                                     ((string.IsNullOrEmpty(alias)
                                                           ? ((file.Contains(".") ? file.Split('.')[0] : file))
                                                           : alias) + ".lnk"));
                    if (File.Exists(targetPath))
                        throw new IOException("Shortcut already exists at/as \"" + targetPath + "\"");
                    ((IPersistFile) link).Save(targetPath, false);
                    return;


                case StartupType.MachineStartupRegistry:
                    RegistryKey add = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (add == null)
                        throw new Exception("Invalid registery path.");
                    add.SetValue(string.IsNullOrEmpty(alias) ? file : alias, "\"" + path + "\"");
                    return;


                case StartupType.LocalUserStartupRegistry:
                    RegistryKey ad = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                                                                     true);
                    if (ad == null)
                        throw new Exception("Invalid registery path.");
                    ad.SetValue(string.IsNullOrEmpty(alias) ? file : alias, "\"" + path + "\"");
                    return;
 
 */