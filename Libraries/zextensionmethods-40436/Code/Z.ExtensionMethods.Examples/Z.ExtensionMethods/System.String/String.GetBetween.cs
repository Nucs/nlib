using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_GetBetween
    {
        [TestMethod]
        public void GetBetween()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string result1 = @this.GetBetween("F", "z"); // return "i";
            string result2 = @this.GetBetween("a", "b"); // return "";

            // Unit Test
            Assert.AreEqual("i", result1);
            Assert.AreEqual("", result2);
        }
    }
}