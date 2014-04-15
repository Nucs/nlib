using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Left
    {
        [TestMethod]
        public void Left()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string value = @this.Left(2); // return "Fi";

            // Unit Test
            Assert.AreEqual("Fi", value);
        }
    }
}