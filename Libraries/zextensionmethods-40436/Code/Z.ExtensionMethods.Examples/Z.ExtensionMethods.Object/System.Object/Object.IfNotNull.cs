using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_IfNotNull
    {
        [TestMethod]
        public void IfNotNull()
        {
            // Type
            var values = new List<string> {"Fizz", "Buzz"};
            List<string> valuesNull = null;

            // Exemples
            string result1 = values.IfNotNull(x => x.First(), "FizzBuzz"); // return "Fizz";
            string result2 = valuesNull.IfNotNull(x => x.First(), "FizzBuzz"); // return "FizzBuzz";
            string result3 = valuesNull.IfNotNull(x => x.First(), () => "FizzBuzz"); // return "FizzBuzz"

            // Unit Test
            Assert.AreEqual("Fizz", result1);
            Assert.AreEqual("FizzBuzz", result2);
            Assert.AreEqual("FizzBuzz", result3);
        }
    }
}