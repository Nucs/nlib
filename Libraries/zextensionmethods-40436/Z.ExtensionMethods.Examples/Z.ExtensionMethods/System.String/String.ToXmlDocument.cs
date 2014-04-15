using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToXmlDocument
    {
        [TestMethod]
        public void ToXmlDocument()
        {
            // Type
            string @this = "<Fizz>Buzz</Fizz>";

            // Examples
            XmlDocument value = @this.ToXmlDocument(); // return a XmlDocument from the specified string.

            // Unit Test
            Assert.AreEqual("<Fizz>Buzz</Fizz>", value.OuterXml);
        }
    }
}