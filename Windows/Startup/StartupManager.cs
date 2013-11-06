using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;

namespace nucs.Windows.Startup {
    public static class StartupManager {
        #region RegisterApplication Overloads

        /// <summary>
        /// Provides 3 different methods\types to register program's
        /// startup to the computer.
        /// </summary>
        /// <param name="regtype">One of the three methods you will</param>
        /// <param name="folder">The folder that the file is located in (w/o the file name itself) for e.g. @"C:\Program Files\"</param>
        /// <param name="file">The file that the shortcut refers to, for e.g. "game.exe" or "2girls1cup.mkv"</param>
        /// <param name="applicationName">Represents the name of the shortcut or the program name in the registry</param>
        public static void RegisterApplication(StartupType regtype, string folder, string file, string applicationName) {
            if (applicationName == null) applicationName = "";
            string path = Path.Combine(folder, file);
            if (string.IsNullOrEmpty(Path.GetFileName(path)))
                throw new InvalidOperationException("Could not find a file name at the combined path \"" + path + "\"");
            if (File.Exists(path) == false)
                throw new IOException("Could not find file for shortcut creation at/in \"" + path + "\"");

            switch (regtype) {
                case StartupType.DefaultStartupFolder:
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
        /// Provides 3 different methods\types to register program's
        /// startup to the computer.
        /// </summary>
        /// <param name="regtype">One of the three methods you will</param>
        /// <param name="fullpath">The folder and the file that is targeted for e.g. @"C:\Program Files\videogame.exe"</param>
        /// <param name="applicationName">Represents the name of the shortcut or the program name in the registry</param>
        public static void RegisterApplication(StartupType regtype, string fullpath, string applicationName) {
            fullpath = fullpath.Replace('/', '\\');
            string[] spl = fullpath.Split('\\');
            string splfolder = fullpath.Remove(fullpath.IndexOf(spl.Last(), StringComparison.OrdinalIgnoreCase),
                                               spl.Last().Length);
            RegisterApplication(regtype, splfolder, spl.Last(), applicationName);
        }

        public static bool RegisterApplicationSafe(StartupType regtype, string folder, string file,
                                                   string applicationName) {
            if (applicationName == null) applicationName = "";
            try {
                RegisterApplication(regtype, folder, file, applicationName);
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        public static bool RegisterApplicationSafe(StartupType regtype, string fullpath, string applicationName) {
            try {
                RegisterApplication(regtype, fullpath, applicationName);
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        #region Nested type: IShellLink

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink {
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
        internal class ShellLink {}

        #endregion

        #endregion

        #region IsRegistered Overloads

        /// <summary>
        /// Tests if the app is registered in all methods and returns if any detected.
        /// </summary>
        /// <param name="applicationName">The application name</param>
        /// <returns>if registered, returns the method, if not; StartupType.Null</returns>
        public static StartupType IsApplicationRegisteredSafe(string applicationName) {
            try {
                return IsApplicationRegistered(applicationName);
            }
            catch {
                return StartupType.Null;
            }
        }

        /// <summary>
        /// Tests if the app is registered in all methods and returns if any detected.
        /// </summary>
        /// <param name="applicationName">The application name</param>
        /// <returns>if registered, returns the method, if not; StartupType.Null</returns>
        public static StartupType IsApplicationRegistered(string applicationName) {
            //case StartupType.DefaultStartupFolder:
                try {
                    if (File.Exists(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                        applicationName + ".lnk")))
                        return StartupType.DefaultStartupFolder;
                } catch (Exception e) {
                    
                }
            //case StartupType.MachineStartupRegistry:
                try {
                    RegistryKey add = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (add == null)
                        throw new Exception("Invalid registery path.");
                    if (add.GetValue(applicationName) != null) return StartupType.MachineStartupRegistry;
                } catch (Exception e) {
                    
                }
            //case StartupType.LocalUserStartupRegistry:
                try {
                    RegistryKey ad = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (ad == null)
                        throw new Exception("Invalid registery path.");
                    if (ad.GetValue(applicationName) != null) return StartupType.LocalUserStartupRegistry;
                } catch (Exception e) {
                    
                }

            return StartupType.Null;
        }

        /// <summary>
        /// Tests if the app is registered in a specific registeration method.
        /// </summary>
        /// <param name="regtype">The registeration method.</param>
        /// <param name="applicationName">The application name</param>
        /// <returns>if registered or not.</returns>
        public static bool IsApplicationRegisteredSafe(StartupType regtype, string applicationName) {
            try {
                return IsApplicationRegistered(regtype, applicationName);
            } catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Tests if the app is registered in a specific registeration method. Beware of exceptions.
        /// </summary>
        /// <param name="regtype">The registeration method.</param>
        /// <param name="applicationName">The application name</param>
        /// <returns>if registered or not.</returns>
        public static bool IsApplicationRegistered(StartupType regtype, string applicationName) {
            if (regtype == StartupType.Null)
                return false;
            switch (regtype) {
                case StartupType.DefaultStartupFolder:
                    return File.Exists(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                                     applicationName + ".lnk")
                        );
                case StartupType.MachineStartupRegistry:
                    RegistryKey add = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    if (add == null)
                        throw new Exception("Invalid registery path.");
                    return add.GetValue(applicationName) != null;
                case StartupType.LocalUserStartupRegistry:
                    RegistryKey ad = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                                                                     true);
                    if (ad == null)
                        throw new Exception("Invalid registery path.");
                    return ad.GetValue(applicationName) != null;
                default:
                    throw new ArgumentOutOfRangeException("regtype");
            }
        }

        #endregion

        #region IsRegistered Overloads

        public static bool UnregisterApplicationSafe(StartupType regtype, string applicationName) {
            if (regtype == StartupType.Null)
                return false;
            try {
                return UnregisterApplication(regtype, applicationName);
            }
            catch (Exception) {
                return false;
            }
        }

        public static bool UnregisterApplication(StartupType regtype, string applicationName) {
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
                    RegistryKey ad = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                                                                     true);
                    if (ad == null)
                        throw new Exception("Invalid registery path.");
                    ad.DeleteValue(applicationName, true);
                    return true;
            }
            return false;
        }

        #endregion

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
                RegisterApplication(StartupType.MachineStartupRegistry, pathToFile, "Nothing");
                if (IsApplicationRegisteredSafe(StartupType.MachineStartupRegistry, "Nothing")) {
                    UnregisterApplicationSafe(StartupType.MachineStartupRegistry, "Nothing");
                    return StartupType.MachineStartupRegistry;
                }
            }
            catch {}
            try {
                RegisterApplication(StartupType.LocalUserStartupRegistry, pathToFile, "Nothing");
                if (IsApplicationRegisteredSafe(StartupType.LocalUserStartupRegistry, "Nothing")) {
                    UnregisterApplicationSafe(StartupType.LocalUserStartupRegistry, "Nothing");
                    return StartupType.LocalUserStartupRegistry;
                }
            }
            catch {}
            try {
                RegisterApplication(StartupType.DefaultStartupFolder, pathToFile, "Nothing");
                if (IsApplicationRegisteredSafe(StartupType.DefaultStartupFolder, "Nothing")) {
                    UnregisterApplicationSafe(StartupType.DefaultStartupFolder, "Nothing");
                    return StartupType.DefaultStartupFolder;
                }
            }
            catch {}
            return StartupType.Null;
        }

        public static StartupType UnregisterApplicationSafe(string applicationName) {
            try {
                StartupType t;
                return UnregisterApplication(t = IsApplicationRegistered(applicationName), applicationName)
                           ? t
                           : StartupType.Null;
            } catch {return StartupType.Null;}
        }

        public static StartupType UnregisterApplication(string applicationName) {
            StartupType t;
            return UnregisterApplication(t = IsApplicationRegistered(applicationName), applicationName) ? t : StartupType.Null;
        }
    }

    public enum StartupType {
        Null = 0,
        DefaultStartupFolder = 1,
        MachineStartupRegistry = 2,
        LocalUserStartupRegistry = 3
    }
}