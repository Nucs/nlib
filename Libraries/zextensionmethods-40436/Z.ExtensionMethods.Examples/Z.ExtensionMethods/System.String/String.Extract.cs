using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Extract
    {
        [TestMethod]
        public void Extract()
        {
            // Type
            string @this = "Fizz1Buzz2";

            // Exemples
            string result = @this.Extract(c => c.IsNumber()); // return "12";

            // Unit Test
            Assert.AreEqual("12", result);
        }
    }
}