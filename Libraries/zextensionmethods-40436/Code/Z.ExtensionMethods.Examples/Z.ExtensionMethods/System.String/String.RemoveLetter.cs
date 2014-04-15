using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_RemoveLetter
    {
        [TestMethod]
        public void RemoveLetter()
        {
            // Type
            string @this = "Fizz1Buzz2";

            // Exemples
            string result = @this.RemoveLetter(); // return "12";

            // Unit Test
            Assert.AreEqual("12", result);
        }
    }
}