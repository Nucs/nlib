using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IDictionary_TKey_TValue_ContainsAllKey
    {
        [TestMethod]
        public void ContainsAllKey()
        {
            // Type
            var @this = new Dictionary<string, string> {{"Fizz", "Buzz"}, {"Fizz2", "Buzz2"}};

            // Exemples
            bool value1 = @this.ContainsAllKey("Fizz", "Fizz2"); // return true;
            bool value2 = @this.ContainsAllKey("Fizz", "Fizz3"); // return false;

            // Unit Test
            Assert.IsTrue(value1);
            Assert.IsFalse(value2);
        }
    }
}