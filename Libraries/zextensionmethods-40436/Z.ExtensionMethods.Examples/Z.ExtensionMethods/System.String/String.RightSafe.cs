using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_RightSafe
    {
        [TestMethod]
        public void RightSafe()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string result1 = @this.RightSafe(2); // return "zz";
            string result2 = @this.RightSafe(int.MaxValue); // return "Fizz";

            // Unit Test
            Assert.AreEqual("zz", result1);
            Assert.AreEqual("Fizz", result2);
        }
    }
}