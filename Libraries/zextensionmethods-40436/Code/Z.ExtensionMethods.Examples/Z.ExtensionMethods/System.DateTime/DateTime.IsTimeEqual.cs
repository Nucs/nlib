using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_IsTimeEqual
    {
        [TestMethod]
        public void IsTimeEqual()
        {
            // Type
            DateTime @thisToday = DateTime.Today;
            DateTime @thisYesterday = @thisToday.AddDays(-1);

            // Exemples
            bool result = @thisYesterday.IsTimeEqual(@thisToday); // return true;

            // Unit Test
            Assert.IsTrue(result);
        }
    }
}