using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ExtractManyDecimal
    {
        [TestMethod]
        public void ExtractManyDecimal()
        {
            // Type

            // Exemples
            decimal[] result1 = "1Fizz-2Buzz".ExtractManyDecimal(); // return new [] {1, -2};
            decimal[] result2 = "12.34Fizz-0.456".ExtractManyDecimal(); // return new [] {12.34, -0.456};

            // Unit Test
            Assert.AreEqual(1M, result1[0]);
            Assert.AreEqual(-2M, result1[1]);
            Assert.AreEqual(12.34M, result2[0]);
            Assert.AreEqual(-0.456M, result2[1]);
        }
    }
}