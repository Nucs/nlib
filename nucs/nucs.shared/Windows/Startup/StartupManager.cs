#if !NETSTANDARD2_0
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using nucs.SystemCore;
using nucs.SystemCore.Boolean;
using nucs.SystemCore.String;
using Z.ExtensionMethods.Object;

namespace nucs.Windows.Startup {
    public static class StartupManager {

        #region Register

        /// <summary>
        ///     Provides 3 different methods\types to register program's
        /// startup to the computer.
        /// </summary>
        /// <param name="regtype">One of the three methods you will</param>
        /// <param name="folder">The folder that the file is located in (w/o the file name itself) for e.g. @"C:\Program Files\"</param>
        /// <param name="file">The file that the shortcut refers to, for e.g. "game.exe" or "2girls1cup.mkv"</param>
        /// <param name="applicationName">Represents the name of the shortcut or the program name in the registry</param>
        public static void Register(StartupType regtype, string folder, string file, string applicationName) {
            if (applicationName == null) applicationName = AppDomain.CurrentDomain.FriendlyName;
            folder = folder.Replace("file:\\", "");
            string path = Path.Combine(folder, file);
            if (string.IsNullOrEmpty(Path.GetFileName(path)))
                throw new InvalidOperationException("Could not find a file name at the combined path \"" + path + "\"");


            switch (regtype) {
                case StartupType.DefaultStartupFolder:
                    if (File.Exists(path) == false)
                        throw new IOException("Could not find file for shortcut creation at/in \"" + path + "\"");
                    var link = (IShellLink) new ShellLink();
                    link.SetDescription("My Description");
                    link.SetPath(path);
                    string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                                                     ((string.IsNullOrEmpty(applicationName)
                                                           ? ((file.Contains(".") ? file.Split('.')[0] : file))
                                                           : applicationName) + ".lnk"));
                    if (File.Exists(targetPath))
                        throw new IOException("Shortcut already exists at/as \"" + targetPath + "\"");
                    ((IPersistFile) link).Save(targetPath, false);
                    return;


                case StartupType.MachineStartupRegistry:
                    RegistryKey add = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (add == null)
                        throw new Exception("Invalid registery path.");
                    add.SetValue(string.IsNullOrEmpty(applicationName) ? file : applicationName, "\"" + path + "\"");
                    return;


                case StartupType.LocalUserStartupRegistry:
                    RegistryKey ad = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                                                                     true);
                    if (ad == null)
                        throw new Exception("Invalid registery path.");
                    ad.SetValue(string.IsNullOrEmpty(applicationName) ? file : applicationName, "\"" + path + "\"");
                    return;


                default:
                    throw new ArgumentOutOfRangeException("regtype");
            }
        }
        
        /// <summary>
        ///     Provides 3 different methods\types to register program's
        /// startup to the computer.
        /// </summary>
        /// <param name="regtype">One of the three methods you will</param>
        /// <param name="info">The file location for e.g. @"C:\Program Files\2girls1cup.mkv" as FileInfo </param>
        /// <param name="applicationName">Represents the name of the shortcut or the program name in the registry</param>
        public static void Register(StartupType regtype, FileInfo info, string applicationName) {
            Register(regtype, info.DirectoryName, info.Name, applicationName);
        }

        /// <summary>
        ///     Provides 3 different methods\types to register program's
        /// startup to the computer.
        /// </summary>
        /// <param name="regtype">One of the three methods you will</param>
        /// <param name="path">The file location for e.g. @"C:\Program Files\2girls1cup.mkv"</param>
        /// <param name="applicationName">Represents the name of the shortcut or the program name in the registry</param>
        public static void Register(StartupType regtype, string path, string applicationName) {
            Register(regtype, Path.GetDirectoryName(path), Path.GetFileName(path), applicationName);
        }

        /// <summary>
        ///     Provides 3 different methods\types to register program's startup to the computer. 
        /// </summary>
        /// <param name="folder">The folder that the file is located in (w/o the file name itself) for e.g. @"C:\Program Files\"</param>
        /// <param name="file">The file that the shortcut refers to, for e.g. "game.exe" or "2girls1cup.mkv"</param>
        /// <param name="applicationName">Represents the name of the shortcut or the program name in the registry</param>
        public static void Register(string folder, string file, string applicationName) {
            Register(GetPreferedAllowedStartupType(Path.Combine(folder, file)), folder, file, applicationName);
        }

        /// <summary>
        ///     Provides 3 different methods\types to register program's startup to the computer. 
        /// </summary>
        /// <param name="path">The file location for e.g. @"C:\Program Files\2girls1cup.mkv"</param>
        /// <param name="applicationName">Represents the name of the shortcut or the program name in the registry</param>
        public static void Register(string path, string applicationName) {
            Register(GetPreferedAllowedStartupType(path), Path.GetDirectoryName(path), Path.GetFileName(path), applicationName);
        }

        #region Shortcut Creating

        #region Nested type: IShellLink

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        public interface IShellLink {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd,
                         int fFlags);

            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);

            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath,
                                 out int piIcon);

            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        #endregion

        #region Nested type: ShellLink

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        public class ShellLink {}

        #endregion

        #endregion

        #endregion

        #region IsRegistered

        /// <summary>
        /// Tests if the app is registered in all methods and returns if any detected.
        /// </summary>
        /// <param name="applicationName">The application name</param>
        /// <returns>if registered, returns the method, if not; StartupType.Null</returns>
        public static AppStartupRegisteration IsRegistered(string applicationName) {
            return GetRegisterations().FirstOrDefault(reg => reg.Name == applicationName);
        }

        #endregion

        #region Unregister

        public static bool Unregister(string applicationName) {
            var t = GetRegisterations().FirstOrDefault(reg => reg.Name.Equals(applicationName));
            return Unregister(t == null ? StartupType.Null : t.Type, applicationName);
        }

        public static bool Unregister(StartupType regtype, string applicationName) {
            if (regtype == StartupType.Null)
                return false;
            switch (regtype) {
                case StartupType.DefaultStartupFolder:
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                                               applicationName + ".lnk");
                    if (File.Exists(path)) {
                        File.Delete(path);
                        return true;
                    }
                    return false;
                case StartupType.MachineStartupRegistry:
                    RegistryKey add = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (add == null)
                        throw new Exception("Invalid registery path.");
                    add.DeleteValue(applicationName, true);
                    return true;
                case StartupType.LocalUserStartupRegistry:
                    var ad = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.Delete);
                    try {
                        Registry.CurrentUser.DeleteValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\" +
                                                         applicationName);
                    }
                    catch (ArgumentException e) {
                        if (e.Message.Contains("No value exists"))
                            return false;
                    }
                    if (ad == null)
                        throw new Exception("Invalid registery path.");
                    ad.DeleteValue(applicationName, true);
                    return true;
            }
            return false;
        }

        #endregion

        #region Tools

        /// <summary>
        /// Tests access for 3 types of startup and returns the most global startup option that is allowed. See 'StartupType'
        /// </summary>
        /// <returns></returns>
        public static StartupType GetPreferedAllowedStartupType() {
            return GetPreferedAllowedStartupType(Environment.SystemDirectory + "\\explorer.exe");
        }

        /// <summary>
        /// Tests access for 3 types of startup and returns the most global startup option that is allowed. See 'StartupType'
        /// </summary>
        /// <returns></returns>
        public static StartupType GetPreferedAllowedStartupType(string pathToFile) {
            try {
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.Default, RegistryRights.WriteKey);
                return StartupType.MachineStartupRegistry;
            } catch (SecurityException) { } 

            try {
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.Default, RegistryRights.WriteKey);
                return StartupType.LocalUserStartupRegistry;
            } catch (SecurityException) { } 


            try {
               new FileIOPermission(FileIOPermissionAccess.Write, Environment.GetFolderPath(Environment.SpecialFolder.Startup)).Demand();
                return StartupType.DefaultStartupFolder;
            } catch (SecurityException) {}

            return StartupType.Null;
        }

        /// <summary>
        /// returns all startup registerations. 
        /// </summary>
        public static IEnumerable<AppStartupRegisteration> GetRegisterations() {
            //read startup folder
            foreach (var file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Startup)).Where(f=>f.EndsWith(".lnk"))) {
                AppStartupRegisteration reg = null;
                try {
                    var target = getShortcutTarget(file);
                    if (target == null) continue;
                    reg = new AppStartupRegisteration(StartupType.DefaultStartupFolder, target, Path.GetFileNameWithoutExtension(file));
                } catch {}
                if (reg != null)
                    yield return reg;
            }
            RegistryKey add;
            //read registry user
            try {
                add = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.Default, RegistryRights.ReadKey);
            } catch (SecurityException) {
                goto _next;
            }
            if (add == null)
                goto _next;
            foreach (var name in add.GetValueNames())
                yield return new AppStartupRegisteration(StartupType.MachineStartupRegistry, add.GetValue(name).As<string>().Trim('\"'), name);
            
            //read registry machine
            _next:
            try {
                add = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.Default, RegistryRights.ReadKey);
            } catch (SecurityException) {
                yield break;
            }

            if (add == null)
                goto _next;
            foreach (var name in add.GetValueNames())
                yield return new AppStartupRegisteration(StartupType.LocalUserStartupRegistry, add.GetValue(name).As<string>().Trim('\"'), name);
        }

        private static string getShortcutTarget(string file) {
            try {
                if (Path.GetExtension(file).ToLower() != ".lnk") 
                    throw new FileFormatException("Supplied file must be a .LNK file");
                FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                using (BinaryReader fileReader = new BinaryReader(fileStream)) {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                    uint flags = fileReader.ReadUInt32();        // Read flags
                    if ((flags & 1) == 1) {                      // Bit 1 set means we have to
                                                                 // skip the shell item ID list
                        fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                        uint offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                        fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                    }
 
                    long fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                    // structure begins
                    uint totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                    fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                    uint fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                    // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin); // Seek to beginning of
                    // base pathname (target)
                    long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2; // read
                    // the base pathname. I don't need the 2 terminating nulls.
                    char[] linkTarget = fileReader.ReadChars((int) pathLength); // should be unicode safe
                    var link = new string(linkTarget);
 
                    int begin = link.IndexOf("\0\0");
                    if (begin > -1) {
                        int end = link.IndexOf("\\\\", begin + 2) + 2;
                        end = link.IndexOf('\0', end) + 1;
 
                        string firstPart = link.Substring(0, begin);
                        string secondPart = link.Substring(end);
 
                        return firstPart + secondPart;
                    } else {
                        return link;
                    }
                }
            } catch {
                return null;
            }
        }

        /// <summary>
        /// Allows you to search for the registeration using and action.
        /// </summary>
        /// <param name="finder">Checks if the reg is the right one and returns bool.</param>
        /// <returns>The found reg otherwise null.</returns>
        public static AppStartupRegisteration FindRegisteration(BoolAction<AppStartupRegisteration> finder) {
            return GetRegisterations().FirstOrDefault(finder.Invoke);
        }

        /// <summary>
        ///     Removes any registers with the same name that does not match the given reg.
        ///         useful when there is a chance that the app might be moved a lot.
        /// </summary>
        /// <param name="reg">The ideal registeration details.</param>
        public static void KillMisfittingRegisters(AppStartupRegisteration reg) {
            var n = GetRegisterations().Where(r => r.Name.Equals(reg.Name));
            foreach (var miss in n.Where(r=>!r.Equals(reg)))
                miss.Unregister();
        }

        /// <summary>
        ///     Creates the startup registeration to the current software and to it's launching path.
        /// </summary>
        public static AppStartupRegisteration CreateToThisExecutable(StartupType type) {
            if (type == StartupType.Null) return null;
            return new AppStartupRegisteration(type, Assembly.GetExecutingAssembly().CodeBase, AppDomain.CurrentDomain.FriendlyName);
        }

        /// <summary>
        ///     Creates the startup registeration to the current software and to it's launching path.
        ///     also attempts to find the best startup type, otherwise - returns null.
        /// </summary>
        public static AppStartupRegisteration CreateToThisExecutable() {
            return new AppStartupRegisteration(GetPreferedAllowedStartupType(Assembly.GetExecutingAssembly().CodeBase), Assembly.GetExecutingAssembly().CodeBase, AppDomain.CurrentDomain.FriendlyName);
        }

        #endregion

    }

    public class AppStartupRegisteration {
        public StartupType Type { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }

        public AppStartupRegisteration() {}

        public AppStartupRegisteration(StartupType type, string path, string name) {
            Name = name;
            Path = path;
            Type = type;
        }

        public bool IsRegistered() {
            return StartupManager.FindRegisteration(t1 => t1.Equals(this)) != null;
        }

        public void Register() {
            StartupManager.Register(Type, Path, Name);
        }

        public void Unregister() {
            StartupManager.Unregister(Type, Name);
        }

        public override string ToString() {
            return (Name ?? "") + "@" + Type + ":" + (Path ?? "");
        }
        protected bool Equals(AppStartupRegisteration other) {
            return Type == other.Type && string.Equals(Path, other.Path) && string.Equals(Name, other.Name);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (int) Type;
                hashCode = (hashCode*397) ^ (Path != null ? Path.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AppStartupRegisteration) obj);
        }
    }

    public enum StartupType {
        Null = 0,
        DefaultStartupFolder = 1,
        MachineStartupRegistry = 2,
        LocalUserStartupRegistry = 3
    }
}
#endif