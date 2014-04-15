using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_IO_FileInfo_Rename
    {
        [TestMethod]
        public void Rename()
        {
            // Type
            var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_IO_FileInfo_Rename.txt"));
            var @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_IO_FileInfo_Rename2.cs"));
            bool result1 = @thisNewFile.Exists;

            // Intialization
            using (FileStream stream = @this.Create())
            {
            }

            // Examples
            @this.Rename("Examples_System_IO_FileInfo_Rename2.cs");

            // Unit Test
            @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_IO_FileInfo_Rename2.cs"));
            bool result2 = @thisNewFile.Exists;

            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
        }
    }
}