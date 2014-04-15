using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IDictionary_TKey_TValue_ContainsAnyKey
    {
        [TestMethod]
        public void ContainsAnyKey()
        {
            // Type
            var @this = new Dictionary<string, string> {{"Fizz", "Buzz"}, {"Fizz2", "Buzz2"}};

            // Exemples
            bool value1 = @this.ContainsAnyKey("Fizz", "Fizz3"); // return true;
            bool value2 = @this.ContainsAnyKey("Fizz3", "Fizz4"); // return false;

            // Unit Test
            Assert.IsTrue(value1);
            Assert.IsFalse(value2);
        }
    }
}