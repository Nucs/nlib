using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToXDocument
    {
        [TestMethod]
        public void ToXDocument()
        {
            // Type
            string @this = "<Fizz>Buzz</Fizz>";

            // Examples
            XDocument value = @this.ToXDocument(); // return a XDocument from the specified string.

            // Unit Test
            Assert.AreEqual("<Fizz>Buzz</Fizz>", value.ToString());
        }
    }
}