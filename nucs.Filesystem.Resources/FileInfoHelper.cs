using System.Drawing;
using System.IO;
using System.Linq;
using Vestris.ResourceLib;

namespace nucs.Filesystem.Resources {
    public static class FileInfoHelper {
        /// <summary>
        ///     Extracts the icon from the target.
        /// </summary>
        public static Icon ExtractIcon(this FileInfo target) {
            return Icon.ExtractAssociatedIcon(target.FullName);
        }

        /// <summary>
        ///     Attempts to duplicate the icon from another file.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="iconsource"></param>
        public static void CopyIcon(this FileInfo target, FileInfo iconsource) {
            if (File.Exists(target.FullName) == false) throw new FileNotFoundException("Couldnt find the given file", target.FullName);
            if (File.Exists(iconsource.FullName) == false) throw new FileNotFoundException("Couldnt find the given file", iconsource.FullName);

            var idr = new IconDirectoryResource();
            try {
                idr.LoadFrom(iconsource.FullName);
            }
            catch {
                CopyResources(target, iconsource);
                return;
            }

            try {
                idr.SaveTo(target.FullName);
            } catch {
                var icon = idr.Icons.OrderByDescending(ico => ico.Height * ico.Width).First();
                icon.SaveTo(target.FullName);
            }
        }

        /// <summary>
        ///     A
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void CopyResources(this FileInfo target, FileInfo source) {
            if (File.Exists(target.FullName) == false) throw new FileNotFoundException("Couldnt find the given file", target.FullName);
            if (File.Exists(source.FullName) == false) throw new FileNotFoundException("Couldnt find the given file", source.FullName);

            var ri = new ResourceInfo();
            ri.Load(source.FullName);
            foreach (var res in ri) {
                try {
                    res.SaveTo(target.FullName);
                } catch { //copy what ever it can. skip errory ones.
                }
            }
            ri.Dispose();
            
        }
    }
}