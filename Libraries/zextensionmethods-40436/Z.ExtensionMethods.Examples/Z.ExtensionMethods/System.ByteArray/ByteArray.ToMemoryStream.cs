using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_ByteArray_ToMemoryStream
    {
        [TestMethod]
        public void ToMemoryStream()
        {
            // Type
            var @this = new Byte[1] {1};

            // Exemples
            MemoryStream result = @this.ToMemoryStream(); // return an iMemoryStream

            // Unit Test
            Assert.AreEqual(1, result.ToArray()[0]);
        }
    }
}