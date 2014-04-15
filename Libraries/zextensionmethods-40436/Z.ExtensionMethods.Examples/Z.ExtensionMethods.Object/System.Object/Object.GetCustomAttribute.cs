using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_GetCustomAttribute
    {
        [TestMethod]
        public void GetCustomAttribute()
        {
            // Type
            var @this = new TestObject();

            // Exemples
            var result1 = @this.GetCustomAttribute<DescriptionAttribute>(true); // return "Test Description";
            object result2 = @this.GetCustomAttribute(typeof (DescriptionAttribute), true); // return "Test Description";

            // Unit Test
            Assert.AreEqual("Test Description", result1.Description);
            Assert.AreEqual("Test Description", ((DescriptionAttribute) result2).Description);
        }

        [Description("Test Description")]
        public class TestObject
        {
        }
    }
}