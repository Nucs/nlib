using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using nucs.Filesystem;
using nucs.SystemCore.Boolean;

namespace nucs.Startup {
    public static class StartupManager {

        #region Cache
        static StartupManager() {
            var baseType = typeof(IStartupMethod);
            var assembly = baseType.Assembly;
            
            var a = assembly.GetTypes();
            var b = a.Where(t => baseType.IsAssignableFrom(t) && !t.IsInterface);
            startupmethods_cache = new List<Type>();
            startupmethods_cache.AddRange(b);
        }

        private static readonly List<Type> startupmethods_cache;
        #endregion

        /// <summary>
        ///     Gets an instance from all of the distributers that are available.
        /// </summary>
        public static List<IStartupMethod> GetNativeStartupMethods() {
            return startupmethods_cache.Select(Activator.CreateInstance)
                .Cast<IStartupMethod>()
                .Where(sm=>sm.Priority>0)
                .OrderBy(sm => sm.Priority).ToList();
        }

        public static List<IStartupMethod> GetWorkingNativeStartupMethods() {
            return GetNativeStartupMethods().Where(sm => sm.IsAttachable).OrderBy(sm=>sm.Priority).ToList();
        }

        /// <summary>
        ///     Attaches the file to the best prioritized startup method.
        /// </summary>
        /// <param name="filename">The file that will start on startup</param>
        /// <param name="alias">The name inwhich the startup will be registered under, used to avoid collisions and can be null.</param>
        public static StartupAttachResult AttachCurrentToBestNative(string alias = null) {
            return AttachBestNative(Paths.ExecutingExe, alias);
        }

        /// <summary>
        ///     Attaches the file to the best prioritized startup method.
        /// </summary>
        /// <param name="filename">The file that will start on startup</param>
        /// <param name="alias">The name inwhich the startup will be registered under, used to avoid collisions and can be null.</param>
        public static StartupAttachResult AttachBestNative(FileInfo filename, string alias = null) {
            if (!File.Exists(filename.FullName)) 
                throw new FileNotFoundException(filename.FullName, nameof(filename));

            return AttachBestNative(new FileCall(filename), alias);
        }

        /// <summary>
        ///     Attaches the file to the best prioritized startup method.
        /// </summary>
        /// <param name="filename">The file that will start on startup</param>
        /// <param name="alias">The name inwhich the startup will be registered under, used to avoid collisions and can be null.</param>
        public static StartupAttachResult AttachBestNative(FileCall filename, string alias=null) {
            alias = alias ?? filename.Alias;
            var methods = GetWorkingNativeStartupMethods();
            if (methods.Any(method=>method.IsAttached(filename))) return StartupAttachResult.AlreadyAttached;
            StartupAttachResult @result = StartupAttachResult.None;
            if (methods.Count==0) return StartupAttachResult.NoMethodsAvailable;
            foreach (var method in methods) {
                try {
                    method.Attach(filename, alias);
#if DEBUG
                    Console.WriteLine("Attached "+alias+":"+filename.FullName+" to method "+method);
#endif
                    return StartupAttachResult.Successful;
                } catch (InvalidOperationException) {
                    @result = StartupAttachResult.FailedWhileAttaching;
                } catch (Exception e) {
                    throw e; //throw unhandled.
                }
            }
            return @result;
        }

        /// <summary>
        /// Disattaches the file from startup attempting all of the methods.
        /// </summary>
        public static bool DisattachCurrentFromNative() {
            return DisattachNative(Paths.ExecutingExe);
        }

        /// <summary>
        /// Disattaches the file from startup attempting all of the methods.
        /// </summary>
        public static bool DisattachNative(FileInfo filename) {
            var methods = GetNativeStartupMethods();
            var result = false;
            foreach (var method in methods) {
                try {
                    if (!method.IsAttached(filename)) continue;

                    method.Disattach(filename);
#if DEBUG
                    Console.WriteLine("Disattached  " + filename.FullName + " from method " + method);
#endif
                    result = true;
                } catch {
                    
                }
            }
            return result;
        }

        /// <summary>
        ///     Enumerate all FileCalls from all the different methods
        /// </summary>
        public static IEnumerable<FileCall> Enumerate() {
            var methods = GetNativeStartupMethods();
            return methods.SelectMany(m => m.Attached);
        }

        /// <summary>
        ///     Enumerate all FileCalls from all the different methods with where filter. 
        /// </summary>
        public static IEnumerable<FileCall> Enumerate(BoolAction<FileCall> where) {
            var methods = GetNativeStartupMethods();
            return methods.SelectMany(m => m.Attached).Where(fc=>where(fc));
        }

        public enum StartupAttachResult:int {
            Successful=1,
            AlreadyAttached=2,
            NoMethodsAvailable=-1,
            FailedWhileAttaching=-2,
            None = 0
        }
        

    }
}