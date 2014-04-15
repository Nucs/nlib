using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_IsDefault
    {
        [TestMethod]
        public void IsDefault()
        {
            // Type
            int intDefault = 0;
            int intNotDefault = 1;

            // Exemples
            bool result1 = intDefault.IsDefault(); // return true;
            bool result2 = intNotDefault.IsDefault(); // return false;

            // Unit Test
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
    }
}