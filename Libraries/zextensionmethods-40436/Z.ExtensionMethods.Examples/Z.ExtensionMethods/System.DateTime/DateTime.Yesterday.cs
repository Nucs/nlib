using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_Yesterday
    {
        [TestMethod]
        public void Yesterday()
        {
            // Type
            DateTime @this = DateTime.Now;

            // Exemples
            DateTime result = @this.Yesterday(); // Return date as yesterday

            // Unit Test
            Assert.AreEqual(@this.AddDays(-1).Day, result.Day);
        }
    }
}