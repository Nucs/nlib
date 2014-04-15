using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Random_CoinToss
    {
        [TestMethod]
        public void CoinToss()
        {
            // Type
            var @this = new Random();

            // Examples
            bool value = @this.CoinToss(); // return true or false at random.
        }
    }
}