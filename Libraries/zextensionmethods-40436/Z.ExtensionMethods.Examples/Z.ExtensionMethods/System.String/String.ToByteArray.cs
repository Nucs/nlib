using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToByteArray
    {
        [TestMethod]
        public void ToByteArray()
        {
            // Type
            string @this = "Fizz";

            // Examples
            byte[] value = @this.ToByteArray(); // return new byte[] { 70, 105, 122, 122 };

            // Unit Test
            Assert.AreEqual(4, value.Length);
        }
    }
}