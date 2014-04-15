using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ConcatWith
    {
        [TestMethod]
        public void ConcatWith()
        {
            // Type
            string @this = "Fizz";

            // Exemples
            string result = @this.ConcatWith("Buzz", "FizzBuzz"); // return "FizzBuzzFizzBuzz";

            // Unit Test
            Assert.AreEqual("FizzBuzzFizzBuzz", result);
        }
    }
}