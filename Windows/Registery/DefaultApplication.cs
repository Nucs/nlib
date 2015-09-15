using System.IO;

namespace nucs.Windows.Registery {
    public static class Registry {
                public static string GetDefaultApplication(string extension) {
            string extensionId = GetClassesRootKeyDefaultValue(extension);
            if (extensionId == null)
                return null;
           

            string openCommand = GetClassesRootKeyDefaultValue(
                    new[] { extensionId, "shell", "open", "command" }.PathCombine());

            if (openCommand == null)
                return null;

            return openCommand
                             .Replace("%1", string.Empty)
                             .Replace("\"", string.Empty)
                             .Trim();
             
        }

        private static string GetClassesRootKeyDefaultValue(string keyPath) {
            using (var key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(keyPath)) {
                if (key == null) 
                    return null;
                
                var defaultValue = key.GetValue(null);
                if (defaultValue == null)
                    return null;

                return defaultValue.ToString();
            }
        }
    }
}