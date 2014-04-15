using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_IsDateEqual
    {
        [TestMethod]
        public void IsDateEqual()
        {
            // Type
            var @thisMorning = new DateTime(2014, 04, 12, 8, 0, 0);
            var @thisAfternoon = new DateTime(2014, 04, 12, 17, 0, 0);

            // Exemples
            bool result = @thisMorning.IsDateEqual(@thisAfternoon); // return true;

            // Unit Test
            Assert.IsTrue(result);
        }
    }
}