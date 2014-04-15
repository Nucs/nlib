using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_To
    {
        [TestMethod]
        public void To()
        {
            string nullValue = null;
            string value = "1";
            object dbNullValue = DBNull.Value;

            // Exemples
            var result1 = value.To<int>(); // return 1;
            var result2 = value.To<int?>(); // return 1;
            var result3 = nullValue.To<int?>(); // return null;
            var result4 = dbNullValue.To<int?>(); // return null;

            // Unit Test
            Assert.AreEqual(1, result1);
            Assert.AreEqual(1, result2.Value);
            Assert.IsFalse(result3.HasValue);
            Assert.IsFalse(result4.HasValue);
        }
    }
}