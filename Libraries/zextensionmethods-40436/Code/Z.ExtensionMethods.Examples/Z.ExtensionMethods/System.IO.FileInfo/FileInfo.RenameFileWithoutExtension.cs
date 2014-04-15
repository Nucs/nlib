using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_IO_FileInfo_RenameFileWithoutExtension
    {
        [TestMethod]
        public void RenameFileWithoutExtension()
        {
            // Type
            var @this = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_IO_FileInfo_RenameWithoutExtension.txt"));
            var @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_IO_FileInfo_RenameWithoutExtension2.txt"));
            bool result1 = @thisNewFile.Exists;

            // Intialization
            using (FileStream stream = @this.Create())
            {
            }

            // Examples
            @this.RenameFileWithoutExtension("Examples_System_IO_FileInfo_RenameWithoutExtension2");

            // Unit Test
            @thisNewFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_IO_FileInfo_RenameWithoutExtension2.txt"));
            bool result2 = @thisNewFile.Exists;

            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
        }
    }
}