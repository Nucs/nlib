using System;
using System.Collections.Generic;
using System.Linq;

namespace nucs.SystemCore.Reflection {
    public static class NamespaceTools {
        /// <summary>
        /// Returns the topmost namespaces from all assemblies that are in the current AppDomain.
        /// </summary>
        public static IEnumerable<string> GetTopLevelNamespaces() {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes())
                       .Select(filter_namespaces)
                       .Distinct().Where(names => !(string.IsNullOrEmpty(names) || names.ContainsAny("<", ">")));
        }

        private static string filter_namespaces(Type t) {
            string ns = t.Namespace ?? "";
            int firstDot = ns.IndexOf('.');
            return firstDot == -1 ? ns : ns.Substring(0, firstDot);
        }
    }
}