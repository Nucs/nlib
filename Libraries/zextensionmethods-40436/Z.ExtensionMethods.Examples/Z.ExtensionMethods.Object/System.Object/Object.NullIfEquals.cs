using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_NullIfEquals
    {
        [TestMethod]
        public void NullIfEquals()
        {
            // Type
            object @this = "1";

            // Exemples
            object result1 = @this.NullIfEquals("1"); // return null;
            object result2 = @this.NullIfEquals("2"); // return "1";

            // Unit Test
            Assert.AreEqual(null, result1);
            Assert.AreEqual("1", result2);
        }
    }
}