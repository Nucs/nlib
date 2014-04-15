using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_RemoveWhere
    {
        [TestMethod]
        public void RemoveWhere()
        {
            // Type
            string @this = "Fizz1Buzz2";

            // Exemples
            string result = @this.RemoveWhere(c => c.IsNumber()); // return "FizzBuzz";

            // Unit Test
            Assert.AreEqual("FizzBuzz", result);
        }
    }
}