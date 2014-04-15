using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_ShallowCopy
    {
        [TestMethod]
        public void ShallowCopy()
        {
            // Type
            var @this = new TestClass {Value = "Fizz"};

            // Exemples
            TestClass clone = @this.ShallowCopy(); // return a shallow copy;

            // Unit Test
            Assert.AreEqual(@this.Value, clone.Value);
        }

        [Serializable]
        public class TestClass
        {
            public string Value;
        }
    }
}