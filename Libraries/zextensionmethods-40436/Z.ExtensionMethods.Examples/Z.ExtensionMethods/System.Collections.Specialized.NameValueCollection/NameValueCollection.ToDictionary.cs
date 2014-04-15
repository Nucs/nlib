using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Specialized_NameValueCollection_ToDictionary
    {
        [TestMethod]
        public void ToDictionary()
        {
            // Type
            var @this = new NameValueCollection {{"Fizz", "Buzz"}};

            // Exemples
            IDictionary<string, object> result = @this.ToDictionary(); // return a Dictionary;

            // Unit Test
            Assert.AreEqual("Buzz", result["Fizz"]);
        }
    }
}