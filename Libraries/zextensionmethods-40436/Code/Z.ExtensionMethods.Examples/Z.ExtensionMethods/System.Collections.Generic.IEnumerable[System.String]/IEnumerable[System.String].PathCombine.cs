using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IEnumerable_System_String_PathCombine
    {
        [TestMethod]
        public void PathCombine()
        {
            // Type
            var @this = new List<string> {"c:\\", "Fizz", "Buzz.txt"};

            // Exemples
            string result = @this.PathCombine(); // return "c:\Fizz\Buzz.txt"

            // Unit Test
            Assert.AreEqual(@"c:\Fizz\Buzz.txt", result);
        }
    }
}