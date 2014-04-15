using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_ToInt32
    {
        [TestMethod]
        public void ToInt32()
        {
            // Type
            string @this = "32";

            // Exemples
            int result = @this.ToInt32(); // return 32;

            // Unit Test
            Assert.AreEqual(32, result);
        }
    }
}