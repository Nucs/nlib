using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_IsEmpty
    {
        [TestMethod]
        public void IsEmpty()
        {
            // Type
            string @thisEmpty = "";
            string @thisNotEmpty = "FizzBuzz";

            // Exemples
            bool result1 = @thisEmpty.IsEmpty(); // return true;
            bool result2 = @thisNotEmpty.IsEmpty(); // return false;

            // Unit Test
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
    }
}