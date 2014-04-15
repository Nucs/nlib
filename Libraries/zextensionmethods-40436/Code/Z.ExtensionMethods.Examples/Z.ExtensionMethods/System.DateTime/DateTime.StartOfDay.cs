using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_StartOfDay
    {
        [TestMethod]
        public void StartOfDay()
        {
            // Type
            DateTime @this = DateTime.Now;

            // Examples
            DateTime value = @this.StartOfDay(); // value = "yyyy/MM/dd 00:00:00:000";

            // Unit Test
            Assert.AreEqual(new DateTime(value.Year, value.Month, value.Day), value);
        }
    }
}