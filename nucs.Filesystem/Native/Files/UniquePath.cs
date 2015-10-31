using System;
using System.Collections.Generic;
using System.IO;
using nucs.SystemCore.Boolean;

namespace nucs.Filesystem {
    /// <summary>
    ///     This class generates a unique directory/file per PC (hardwares)
    /// </summary>
    public static class UniquePath {
        public static readonly List<DirectoryInfo> PotentialPaths = new List<DirectoryInfo> {
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)),
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)),
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)),
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)),
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
        };

        public static DirectoryInfo GetUniqueDirectory {
            get {
                var rand = UniquePcId.NewUniqueRandomizer;
                _retry:
                var random_dir = PotentialPaths[rand.Next(PotentialPaths.Count)];
                if (random_dir.IsDirectoryWritable() == false)
                    goto _retry;
                return random_dir;
            }
        }

        public static string GetUniqueFileName {
            get {
                var rand = UniquePcId.NewUniqueRandomizer;
                return StringGenerator.Generate(rand, 10);
            }
        }

        /// <summary>
        ///     Fetches a unique file location.
        /// </summary>
        /// <returns></returns>
        public static FileInfo FetchPath() {
            return new FileInfo(Path.Combine(GetUniqueDirectory.FullName, GetUniqueFileName));
        }
         
    }
}