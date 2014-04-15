using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_RemoveDiacritics
    {
        [TestMethod]
        public void RemoveDiacritics()
        {
            // Type
            string @this = "יטךפמג";

            // Examples
            string value = @this.RemoveDiacritics(); // return "eeeoia";

            // Unit Test
            Assert.AreEqual("eeeoia", value);
        }
    }
}