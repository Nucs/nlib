using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_Age
    {
        [TestMethod]
        public void Age()
        {
            // Type
            var @this = new DateTime(1981, 01, 01);

            // Exemples
            int result = @this.Age(); // result CurrentYear - 1981

            // Unit Test
            Assert.AreEqual(DateTime.Now.Year - 1981, result);
        }
    }
}