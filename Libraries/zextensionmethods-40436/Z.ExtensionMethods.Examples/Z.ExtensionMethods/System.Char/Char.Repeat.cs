using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Char_Repeat
    {
        [TestMethod]
        public void Repeat()
        {
            // Type
            char @this = 'F';

            // Examples
            string result = @this.Repeat(3); // return "FFF";

            // Unit Test
            Assert.AreEqual("FFF", result);
        }
    }
}