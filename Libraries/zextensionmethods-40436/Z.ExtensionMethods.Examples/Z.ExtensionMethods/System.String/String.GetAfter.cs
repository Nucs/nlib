using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_GetAfter
    {
        [TestMethod]
        public void GetAfter()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string result1 = @this.GetAfter("i"); // return "zz";
            string result2 = @this.GetAfter("a"); // return "";

            // Unit Test
            Assert.AreEqual("zz", result1);
            Assert.AreEqual("", result2);
        }
    }
}