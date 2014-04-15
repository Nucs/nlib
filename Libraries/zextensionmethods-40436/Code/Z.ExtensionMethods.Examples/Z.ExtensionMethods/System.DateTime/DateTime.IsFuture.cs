using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_IsFuture
    {
        [TestMethod]
        public void IsFuture()
        {
            // Type
            DateTime @this = DateTime.Now.AddDays(1);

            // Examples
            bool value1 = @this.IsFuture(); // return true;
            bool value2 = @this.AddYears(-1).IsFuture(); // return false;

            // Unit Test
            Assert.IsTrue(value1);
            Assert.IsFalse(value2);
        }
    }
}