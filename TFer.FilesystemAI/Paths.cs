using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using nucs.Collections;
using nucs.Mono.System.Threading;
using nucs.SystemCore.Boolean;
using TFer.Filesystem.AI;
using TFer.Filesystem.Distribution;

namespace TFer.Filesystem {

    /// <summary>
    ///     Class that determines paths.
    /// </summary>
    public static partial class Paths {
        private static VoidTask _cacheprogress = null;
        public static void Cache() {
            _cacheprogress?.Wait();

            _cacheprogress = Task.Run(() => {
                var l = new List<VoidTask>();
                foreach (var dist in DistributionManager.GetDistributers(false)) {
                    l.Add(Task.Run(() => {
                        dist.Distributables().ForEach(d=>_randlocdb.Add(d)).EvaluateLinq();
                    }));
                }
                l.ToArray().WaitAll();
                _cacheprogress = null;
            });
        }

        /// <summary>
        /// Gives the path to windows dir, most likely to be 'C:/Windows/'
        /// </summary>
        public static DirectoryInfo WindowsDir => new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.System));
        
        /// <summary>
        ///     The exe that has started this process
        /// </summary>
        public static FileInfo ExecutingExe => new FileInfo(Assembly.GetEntryAssembly().Location);

        private static readonly object _randloclock = new object();
        private static readonly Collector<DirectoryInfo> _randlocdb = new Collector<DirectoryInfo>();
        /// <summary>
        ///     Will generate a clever random location.
        /// </summary>
        public static FileInfo RandomLocation {
            get {
                lock (_randloclock) {
                    _reload:
                    Cache();
                    while (_randlocdb.ItemsLeft == 0)
                        Paths._cacheprogress.Wait(100);

                    while (_randlocdb.ItemsLeft>0) {
                        var randloc = _randlocdb.TakeRandom();
                        randloc.EnsureDirectoryExists();
                        var fn = AIFileNameGenerator.Generate(randloc);
                        if (File.Exists(fn.FullName))
                            continue;
                        try { //attempt writing
                            fn.Create().Close();
                            fn.Delete();
                        } catch {
                            continue;
                        }
                        return fn;
                    }
                    goto _reload;
                }
            }
        }
        
        /// <summary>
        ///     Will generate a writable directory.
        /// </summary>
        public static DirectoryInfo RandomDirectory {
            get {
                lock (_randloclock) {
                    _reload:
                    Cache();
                    while (_randlocdb.ItemsLeft == 0)
                        Paths._cacheprogress.Wait(100);
                    
                    while (_randlocdb.ItemsLeft>0) {
                        var randloc = _randlocdb.TakeRandom();
                        randloc.EnsureDirectoryExists();
                        var fn = new FileInfo(Path.Combine(randloc.FullName, StringGenerator.Generate(10)));
                        try { //attempt writing
                            fn.Create().Close();
                            fn.Delete();
                        } catch {
                            continue;
                        }
                        return randloc;
                    }
                    goto _reload;
                }
            }
        }

        
        /// <summary>
        /// Checks the ability to create and write to a file in the supplied directory.
        /// </summary>
        /// <param name="directory">String representing the directory path to check.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public static bool IsDirectoryWritable(this DirectoryInfo directory) {
            bool success = false;
            string fullPath = directory + "testicales.exe";

            if (directory.Exists) {
                try {
                    using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew,
                        FileAccess.Write)) {
                        fs.WriteByte(0xff);
                    }

                    if (File.Exists(fullPath)) {
                        File.Delete(fullPath);
                        success = true;
                    }
                }
                catch (Exception) {
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        ///     Fetches a unique location for this PC that is writable with the current credentials.
        /// </summary>
        public static DirectoryInfo UniqueWritableLocation => UniquePath.GetUniqueDirectory;

        
        /// <summary>
        ///     Combines the file name with the dir of <see cref="Paths.ExecutingExe"/>, resulting in path of a file inside the directory of the executing exe file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static FileInfo CombineToExecutingBase(string filename) {
            if (Paths.ExecutingExe.DirectoryName != null) return new FileInfo(Path.Combine(Paths.ExecutingExe.DirectoryName, filename));
            return null;
        }

    }
}