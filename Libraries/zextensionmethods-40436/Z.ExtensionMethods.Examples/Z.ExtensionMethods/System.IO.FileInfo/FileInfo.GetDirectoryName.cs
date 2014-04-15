using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_IO_FileInfo_GetDirectoryName
    {
        [TestMethod]
        public void GetDirectoryName()
        {
            // Type
            var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DirectoryInfo_GetDirectoryName", "CreateDirectory.txt"));

            // Examples
            string result = @this.GetDirectoryName(); // return "DirectoryInfo_GetDirectoryName";

            // Unit Test
            Assert.AreEqual(@this.Directory.Name, result);
        }
    }
}