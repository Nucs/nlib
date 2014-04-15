using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_IsPast
    {
        [TestMethod]
        public void IsPast()
        {
            // Type
            DateTime @this = DateTime.Now.AddDays(1);

            // Examples
            bool value1 = @this.IsPast(); // return false;
            bool value2 = @this.AddYears(-1).IsPast(); // return true;

            // Unit Test
            Assert.IsFalse(value1);
            Assert.IsTrue(value2);
        }
    }
}