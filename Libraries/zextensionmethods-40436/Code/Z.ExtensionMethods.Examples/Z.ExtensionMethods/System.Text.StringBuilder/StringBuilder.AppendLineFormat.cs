using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Text_StringBuilder_AppendLineFormat
    {
        [TestMethod]
        public void AppendLineFormat()
        {
            // Type
            var @this = new StringBuilder();

            // Exemples
            @this.AppendLineFormat("{0}{1}", "Fizz", "Buzz"); // return "FizzBuzz";

            // Unit Test
            Assert.AreEqual("FizzBuzz" + Environment.NewLine, @this.ToString());
        }
    }
}