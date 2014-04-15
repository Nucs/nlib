using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToFileInfo
    {
        [TestMethod]
        public void ToFileInfo()
        {
            // Type
            string @this = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Examples_System_String_ToFileInfo.txt");

            // Examples
            FileInfo value = @this.ToFileInfo(); // return a FileInfo for the specified path.

            // Unit Test
            Assert.AreEqual(@this, value.FullName);
        }
    }
}