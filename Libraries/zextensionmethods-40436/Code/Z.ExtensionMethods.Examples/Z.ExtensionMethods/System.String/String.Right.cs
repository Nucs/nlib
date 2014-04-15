using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Right
    {
        [TestMethod]
        public void Right()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string value = @this.Right(2); // return "zz";

            // Unit Test
            Assert.AreEqual("zz", value);
        }
    }
}