using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_TimeSpan_UtcAgo
    {
        [TestMethod]
        public void UtcAgo()
        {
            // Type
            var @this = new TimeSpan(1, 0, 0, 0);

            // Examples
            DateTime value = @this.UtcAgo(); // return yesterday.

            // Unit Test
            Assert.IsTrue(DateTime.UtcNow.Subtract(value).Days >= 1);
        }
    }
}