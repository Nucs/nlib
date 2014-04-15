using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_IsAlpha
    {
        [TestMethod]
        public void IsAlpha()
        {
            // Type
            string @thisAlpha = "abc";
            string @thisNotAlpha = "abc123";

            // Examples
            bool value1 = @thisAlpha.IsAlpha(); // return true;
            bool value2 = @thisNotAlpha.IsAlpha(); // return false;

            // Unit Test
            Assert.IsTrue(value1);
            Assert.IsFalse(value2);
        }
    }
}