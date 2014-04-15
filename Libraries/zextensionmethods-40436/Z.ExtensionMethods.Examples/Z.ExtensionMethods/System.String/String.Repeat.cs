using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_Repeat
    {
        [TestMethod]
        public void Repeat()
        {
            // Type
            string @this = "Fizz";

            // Examples
            string value = @this.Repeat(3); // return "FizzFizzFizz";

            // Unit Test
            Assert.AreEqual("FizzFizzFizz", value);
        }
    }
}