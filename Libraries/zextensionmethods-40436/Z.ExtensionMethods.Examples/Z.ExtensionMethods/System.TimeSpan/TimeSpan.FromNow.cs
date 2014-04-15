using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_TimeSpan_FromNow
    {
        [TestMethod]
        public void FromNow()
        {
            // Type
            var @this = new TimeSpan(1, 0, 0, 0);

            // Examples
            DateTime value = @this.FromNow(); // return tomorrow.

            // Unit Test
            Assert.IsTrue(DateTime.Now.Subtract(value).Days <= -1);
        }
    }
}