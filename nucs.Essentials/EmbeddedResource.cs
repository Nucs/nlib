using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace nucs.Essentials
{
    public static class EmbeddedResource {
        /// <summary>
        ///     Finds the resource in calling assembly that contains the given string 'name'.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="assembly">Specific assembly, leave null for calling assembly</param>
        /// <returns></returns>
        public static Stream GetResource(string name, Assembly assembly=null) {
            var asm = assembly ?? Assembly.GetCallingAssembly();
            var target = asm.GetManifestResourceNames().FirstOrDefault(mrn => mrn.Contains(name));
            if (target == null) throw new FileNotFoundException($"Could not find a resource that contains the name '{name}'");
            return asm.GetManifestResourceStream(target);
        }

        /// <summary>
        ///     Exports the zip stored file to wanted dir.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="resourcename"></param>
        public static void ExportZipResource(DirectoryInfo dir, string resourcename, Assembly assembly = null) {
            if (dir == null) throw new ArgumentNullException("dir");
            if (dir.Exists == false) dir.Create();
            ExportZipResource(dir.FullName,resourcename, assembly);
            
        }

        /// <summary>
        ///     Exports the zip stored file to wanted dir.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="resourcename"></param>
        public static void ExportZipResource(string dir, string resourcename, Assembly assembly = null) {
            var fz = new FastZip();
            var stream = GetResource(resourcename, assembly);
            fz.ExtractZip(stream,dir,FastZip.Overwrite.Always,name => true, null,null,true,true);
        }
    }
}
