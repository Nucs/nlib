using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IEnumerable_System_IO_DirectoryInfo_Delete
    {
        [TestMethod]
        public void Delete()
        {
            // Type
            var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "System_Collections_Generic_IEnumerable_System_IO_DirectoryInfo_Delete"));
            Directory.CreateDirectory(root.FullName);
            root.CreateSubdirectory("DirFizz123");
            root.CreateSubdirectory("DirBuzz123");
            root.CreateSubdirectory("DirNotFound123");

            // Exemples
            DirectoryInfo[] directories = root.GetDirectories();
            directories.Delete();

            // Unit Test
            Assert.AreEqual(3, directories.Length);
            Assert.AreEqual(0, root.GetDirectories().Length);
        }
    }
}