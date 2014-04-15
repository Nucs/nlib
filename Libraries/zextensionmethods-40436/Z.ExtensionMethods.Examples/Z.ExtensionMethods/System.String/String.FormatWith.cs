using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_FormatWith
    {
        [TestMethod]
        public void FormatWith()
        {
            // Type
            string @this = "{0}{1}";

            // Examples
            string value = @this.FormatWith("Fizz", "Buzz"); // return "FizzBuzz";

            // Unit Test
            Assert.AreEqual("FizzBuzz", value);
        }
    }
}