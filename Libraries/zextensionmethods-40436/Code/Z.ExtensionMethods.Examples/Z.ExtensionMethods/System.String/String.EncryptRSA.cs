using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_EncryptRSA
    {
        [TestMethod]
        public void EncryptRSA()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string value = @this.EncryptRSA("Buzz"); // return Encrypted string;

            // Unit Test
            Assert.AreEqual("Fizz", value.DecryptRSA("Buzz"));
        }
    }
}