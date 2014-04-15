using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Split
    {
        [TestMethod]
        public void Split()
        {
            // Type
            string @this = "FizzBuzz";

            // Examples
            string[] value = @this.Split("B"); // return new[] {"Fizz", "uzz"}

            // Unit Test
            Assert.AreEqual("Fizz", value[0]);
            Assert.AreEqual("uzz", value[1]);
        }
    }
}