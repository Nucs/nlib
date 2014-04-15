using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ExtractManyDouble
    {
        [TestMethod]
        public void ExtractManyDouble()
        {
            // Type

            // Exemples
            double[] result1 = "1Fizz-2Buzz".ExtractManyDouble(); // return new [] {1, -2};
            double[] result2 = "12.34Fizz-0.456".ExtractManyDouble(); // return new [] {12.34, -0.456};

            // Unit Test
            Assert.AreEqual(1, result1[0]);
            Assert.AreEqual(-2, result1[1]);
            Assert.AreEqual(12.34, result2[0]);
            Assert.AreEqual(-0.456, result2[1]);
        }
    }
}