using System.IO;
using System.Linq;
using System.Reflection;

namespace TFer.Modules.DllLoader.Resource {
    internal static class ResourceHelper {

        /// <summary>
        ///     Finds the resource in calling assembly that contains the given string 'name'.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Stream GetResource(string name) {
            var asm = Assembly.GetCallingAssembly();
            var target = asm.GetManifestResourceNames().FirstOrDefault(mrn => mrn.Contains(name));
            if (target==null) throw new FileNotFoundException($"Could not find a resource that contains the name '{name}'");
            return asm.GetManifestResourceStream(target);
        }
         
    }
}