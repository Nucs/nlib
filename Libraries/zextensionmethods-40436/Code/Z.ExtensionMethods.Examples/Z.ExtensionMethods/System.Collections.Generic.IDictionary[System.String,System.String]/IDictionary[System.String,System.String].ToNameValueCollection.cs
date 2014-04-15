using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IDictionary_System_String_System_String_ToNameValueCollection
    {
        [TestMethod]
        public void ToNameValueCollection()
        {
            // Type
            var @this = new Dictionary<string, string> {{"Fizz", "Buzz"}};

            // Exemples
            NameValueCollection result = @this.ToNameValueCollection(); // return a NameValueCollection

            // Unit Test
            Assert.AreEqual("Buzz", result["Fizz"]);
        }
    }
}