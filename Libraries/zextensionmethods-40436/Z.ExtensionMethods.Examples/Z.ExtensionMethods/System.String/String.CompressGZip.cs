using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_CompressGZip
    {
        [TestMethod]
        public void CompressGZip()
        {
            // Type
            string @this = "FizzBuzz";

            // Exemples
            var result = @this.CompressGZip();

            // Unit Test
            Assert.AreEqual("FizzBuzz", result.DecompressGZip());
        }
    }
}