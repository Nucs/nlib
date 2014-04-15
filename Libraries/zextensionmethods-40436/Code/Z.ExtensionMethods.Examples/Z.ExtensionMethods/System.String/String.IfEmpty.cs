using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_IfEmpty
    {
        [TestMethod]
        public void IfEmpty()
        {
            // Type
            string @this = "";

            // Exemples
            string result = @this.IfEmpty("FizzBuzz"); // return "FizzBuzz";

            // Unit Test
            Assert.AreEqual("FizzBuzz", result);
        }
    }
}