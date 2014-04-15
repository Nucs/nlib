using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_Tomorrow
    {
        [TestMethod]
        public void Tomorrow()
        {
            // Type
            DateTime @this = DateTime.Now;

            // Exemples
            DateTime result = @this.Tomorrow(); // Return date as tomorrow

            // Unit Test
            Assert.AreEqual(@this.AddDays(1).Day, result.Day);
        }
    }
}