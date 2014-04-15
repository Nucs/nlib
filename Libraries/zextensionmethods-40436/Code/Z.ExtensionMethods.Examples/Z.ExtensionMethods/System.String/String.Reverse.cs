using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Reverse
    {
        [TestMethod]
        public void Reverse()
        {
            // Type
            string @this = "FizzBuzz";

            // Examples
            string value = @this.Reverse(); // return "zzuBzziF";

            // Unit Test
            Assert.AreEqual("zzuBzziF", value);
        }
    }
}