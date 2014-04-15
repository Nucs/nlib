using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ExtractNumber
    {
        [TestMethod]
        public void ExtractNumber()
        {
            // Type
            string @this = "Fizz1Buzz2";

            // Exemples
            string result = @this.ExtractNumber(); // return "12";

            // Unit Test
            Assert.AreEqual("12", result);
        }
    }
}