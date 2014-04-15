using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_ByteArray_DecompressGZip
    {
        [TestMethod]
        public void DecompressGZip()
        {
            // Type
            byte[] @this = "FizzBuzz".CompressGZip();

            // Exemples
            string result = @this.DecompressGZip(); // return "FizzBuzz";

            // Unit Test
            Assert.AreEqual("FizzBuzz", result);
        }
    }
}