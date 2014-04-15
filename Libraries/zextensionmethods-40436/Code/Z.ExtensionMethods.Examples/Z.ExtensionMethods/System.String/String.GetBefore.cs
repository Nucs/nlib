using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_GetBefore
    {
        [TestMethod]
        public void GetBefore()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string result1 = @this.GetBefore("i"); // return "F";
            string result2 = @this.GetBefore("a"); // return "";

            // Unit Test
            Assert.AreEqual("F", result1);
            Assert.AreEqual("", result2);
        }
    }
}