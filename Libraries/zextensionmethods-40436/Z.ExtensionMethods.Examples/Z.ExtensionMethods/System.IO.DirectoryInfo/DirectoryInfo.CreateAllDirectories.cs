using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_IO_DirectoryInfo_CreateAllDirectory
    {
        [TestMethod]
        public void CreateAllDirectory()
        {
            // Type
            var @this = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DirectoryInfo_Clear", "SubDirectory"));

            // Examples
            @this.CreateAllDirectories(); // Create the directory the first time.

            // Unit Test
            Assert.IsTrue(@this.Exists);
        }
    }
}