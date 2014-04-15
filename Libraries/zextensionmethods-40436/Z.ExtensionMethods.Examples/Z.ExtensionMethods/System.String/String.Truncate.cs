using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Truncate
    {
        [TestMethod]
        public void Truncate()
        {
            // Type
            string @this = "123456789";

            // Exemples
            string result = @this.Truncate(6); // return "123...";

            // Unit Test
            Assert.AreEqual("123...", result);
        }
    }
}