using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_SetFieldValue
    {
        [TestMethod]
        public void SetFieldValue()
        {
            // Type
            var @this = new TestClass();

            // Exemples
            @this.SetFieldValue("PublicField", 1);
            @this.SetFieldValue("InternaStaticlField", 2);

            // Unit Test
            Assert.AreEqual(1, @this.PublicField);
            Assert.AreEqual(2, TestClass.InternaStaticlField);
        }

        public class TestClass
        {
            internal static int InternaStaticlField = 2;
            public int PublicField = 1;
        }
    }
}