using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nucs.Filesystem.Distribution.Methods {
    public class RootDriveDistributer : SpecificDirectoryDistributer {
        protected override IEnumerable<DirectoryInfo> FetchBaseDirs() {
            var all = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).Select(d=>d.RootDirectory.FullName);
            var removeables = DriveInfo.GetDrives().Select(d=>d.RootDirectory.FullName);
            return all.Except(removeables).Select(filtered=>new DirectoryInfo(filtered));
        }

        public override bool IsStatic => false;
    }
}