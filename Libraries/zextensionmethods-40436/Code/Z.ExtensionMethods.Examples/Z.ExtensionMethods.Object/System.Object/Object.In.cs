using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_In
    {
        [TestMethod]
        public void In()
        {
            // Type
            string @this = "a";

            // Examples
            bool value1 = @this.In("a", "1", "2", "3"); // return true;
            bool value2 = @this.In("1", "2", "3"); // return false;

            // Unit Test
            Assert.IsTrue(value1);
            Assert.IsFalse(value2);
        }
    }
}