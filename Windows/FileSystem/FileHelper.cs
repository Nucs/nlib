﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace nucs.Windows.FileSystem
{
    public static class FileHelper {
        public static DirectoryInfo CopyTo(this DirectoryInfo sourceDir, string destinationPath, bool overwrite = false) {
            var sourcePath = sourceDir.FullName;

            var destination = new DirectoryInfo(destinationPath);

            destination.Create();

            foreach (var sourceSubDirPath in Directory.EnumerateDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(sourceSubDirPath.Replace(sourcePath, destinationPath));

            foreach (var file in Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories))
                File.Copy(file, file.Replace(sourcePath, destinationPath), overwrite);

            return destination;
        }

        public static bool OutputStringToFile(this string text, string path) {
            try {
                File.WriteAllText(path, text, Encoding.UTF8);
                return true;
            } catch {
                return false;
            }
        }




    }
}
