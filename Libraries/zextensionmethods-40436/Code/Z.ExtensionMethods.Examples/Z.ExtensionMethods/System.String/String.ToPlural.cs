using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToPlural
    {
        [TestMethod]
        public void ToPlural()
        {
            // Type
            string @this = "mouse";

            // Exemples
            string result = @this.ToPlural(); // return "mice";

            // Unit Test
            Assert.AreEqual("mice", result);
        }
    }
}