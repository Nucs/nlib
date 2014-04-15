using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IDictionary_System_String_System_Object_ToExpando
    {
        [TestMethod]
        public void ToExpando()
        {
            // Type
            var @this = new Dictionary<string, object> {{"Fizz", "Buzz"}};

            // Exemples
            dynamic result = @this.ToExpando(); // return an expando object;

            // Unit Test
            Assert.AreEqual("Buzz", result.Fizz);
        }
    }
}