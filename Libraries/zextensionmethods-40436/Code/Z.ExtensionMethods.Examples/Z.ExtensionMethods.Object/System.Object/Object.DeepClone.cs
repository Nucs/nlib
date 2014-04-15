using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_DeepClone
    {
        [TestMethod]
        public void DeepClone()
        {
            // Type
            var @this = new TestClass {Value = "Fizz"};

            // Exemples
            TestClass clone = @this.DeepClone(); // return a deep clone;

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