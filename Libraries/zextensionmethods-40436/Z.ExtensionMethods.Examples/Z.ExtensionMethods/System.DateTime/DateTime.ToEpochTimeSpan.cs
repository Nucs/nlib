using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_ToEpochTimeSpan
    {
        [TestMethod]
        public void ToEpochTimeSpan()
        {
            // Type
            DateTime @this = DateTime.Now;

            // Exemples
            TimeSpan result = @this.ToEpochTimeSpan(); // return TimeSpan

            // Unit Test
            Assert.AreEqual(@this.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, result.TotalMilliseconds);
        }
    }
}