using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_IsNotEmpty
    {
        [TestMethod]
        public void IsNotEmpty()
        {
            // Type
            string @thisEmpty = "";
            string @thisNotEmpty = "FizzBuzz";

            // Exemples
            bool result1 = @thisEmpty.IsNotEmpty(); // return false;
            bool result2 = @thisNotEmpty.IsNotEmpty(); // return true;

            // Unit Test
            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
        }
    }
}