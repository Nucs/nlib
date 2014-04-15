using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_IsNullOrEmpty
    {
        [TestMethod]
        public void IsNullOrEmpty()
        {
            // Type
            string @thisValue = "Fizz";
            string @thisNull = null;

            // Examples
            bool value1 = @thisValue.IsNullOrEmpty(); // return false;
            bool value2 = @thisNull.IsNullOrEmpty(); // return true;

            // Unit Test
            Assert.IsFalse(value1);
            Assert.IsTrue(value2);
        }
    }
}