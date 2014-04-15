using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ReplaceByEmpty
    {
        [TestMethod]
        public void ReplaceByEmpty()
        {
            // Type
            string @this = "FizzBuzz";

            // Examples
            string value = @this.ReplaceByEmpty("z"); // return "FiBu";

            // Unit Test
            Assert.AreEqual("FiBu", value);
        }
    }
}