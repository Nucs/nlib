using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToDirectoryInfo
    {
        [TestMethod]
        public void ToDirectoryInfo()
        {
            // Type
            string @this = AppDomain.CurrentDomain.BaseDirectory;

            // Examples
            DirectoryInfo value = @this.ToDirectoryInfo(); // return a DirectoryInfo from the specified path.

            // Unit Test
            Assert.AreEqual(@this, value.FullName);
        }
    }
}