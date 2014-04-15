using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ExtractDouble
    {
        [TestMethod]
        public void ExtractDouble()
        {
            // Type

            // Exemples
            double result1 = "Fizz 123 Buzz".ExtractDouble(); // return 123;
            double result2 = "Fizz -123 Buzz".ExtractDouble(); // return -123;
            double result3 = "-Fizz 123 Buzz".ExtractDouble(); // return 123;
            double result4 = "Fizz 123.456 Buzz".ExtractDouble(); // return 123.456;
            double result5 = "Fizz -123Fizz.Buzz456 Buzz".ExtractDouble(); // return -123.456;

            // Unit Test
            Assert.AreEqual(123, result1);
            Assert.AreEqual(-123, result2);
            Assert.AreEqual(123, result3);
            Assert.AreEqual(123.456, result4);
            Assert.AreEqual(-123.456, result5);
        }
    }
}