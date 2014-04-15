using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IEnumerable_System_IO_FileInfo_ForEach
    {
        [TestMethod]
        public void ForEach()
        {
            // Type
            var root = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "System_Collections_Generic_IEnumerable_System_IO_FileInfo_Delete"));
            Directory.CreateDirectory(root.FullName);

            var file1 = new FileInfo(Path.Combine(root.FullName, "test.txt"));
            var file2 = new FileInfo(Path.Combine(root.FullName, "test.cs"));
            var file3 = new FileInfo(Path.Combine(root.FullName, "test.asp"));
            using (FileStream f = file1.Create())
            {
            }
            using (FileStream f = file2.Create())
            {
            }
            using (FileStream f = file3.Create())
            {
            }

            // Exemples
            FileInfo[] files = root.GetFiles();
            files.ForEach(x => x.Delete());

            // Unit Test
            Assert.AreEqual(3, files.Length);
            Assert.AreEqual(0, root.GetFiles().Length);
        }
    }
}