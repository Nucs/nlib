using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_ToNullableInt32
    {
        [TestMethod]
        public void ToNullableInt32()
        {
            // Type
            object @this = null;
            object @thisValue = "32";

            // Exemples
            int? result1 = @this.ToNullableInt32(); // return null;
            int? result2 = @thisValue.ToNullableInt32(); // return 32;

            // Unit Test
            Assert.IsFalse(result1.HasValue);
            Assert.AreEqual(32, result2.Value);
        }
    }
}