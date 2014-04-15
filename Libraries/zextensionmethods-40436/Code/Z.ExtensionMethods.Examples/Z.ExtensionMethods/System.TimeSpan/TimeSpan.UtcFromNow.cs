using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_TimeSpan_UtcFromNow
    {
        [TestMethod]
        public void UtcFromNow()
        {
            // Type
            var @this = new TimeSpan(1, 0, 0, 0);

            // Examples
            DateTime value = @this.UtcFromNow(); // return yesterday.

            // Unit Test
            Assert.IsTrue(DateTime.UtcNow.Subtract(value).Days <= -1);
        }
    }
}