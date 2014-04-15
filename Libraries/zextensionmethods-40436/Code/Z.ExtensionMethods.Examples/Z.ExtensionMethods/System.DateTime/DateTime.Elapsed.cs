using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_DateTime_Elapsed
    {
        [TestMethod]
        public void Elapsed()
        {
            // Type
            DateTime @this = DateTime.Now;

            // Exemples
            TimeSpan result = @this.Elapsed();

            // UnitTest
        }
    }
}