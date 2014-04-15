using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_SetTime
    {
        [TestMethod]
        public void SetTime()
        {
            // Type
            DateTime @thisToday = DateTime.Today;

            // Exemples
            DateTime result = @thisToday.SetTime(15); // Set hours to 15

            // Unit Test
            Assert.AreEqual(15, result.Hour);
        }
    }
}