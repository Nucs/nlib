using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Br2Nl
    {
        [TestMethod]
        public void Br2Nl()
        {
            // Type
            string @this = "Fizz<br />Buzz";

            // Exemples
            string result = @this.Br2Nl(); // return "Fizz/r/nBuzz";

            // Unit Test
            Assert.AreEqual("Fizz" + Environment.NewLine + "Buzz", result);
        }
    }
}