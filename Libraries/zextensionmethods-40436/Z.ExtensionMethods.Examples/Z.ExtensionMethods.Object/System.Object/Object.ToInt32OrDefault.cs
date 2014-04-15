using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.Object;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Object_ToInt32OrDefault
    {
        [TestMethod]
        public void ToInt32OrDefault()
        {
            // Type
            string @thisValid = "32";
            string @thisInvalid = "FizzBuzz";

            // Exemples
            int result1 = @thisValid.ToInt32OrDefault(); // return 32;
            int result2 = @thisInvalid.ToInt32OrDefault(); // return 0;
            int result3 = @thisInvalid.ToInt32OrDefault(-1); // return -1;
            int result4 = @thisInvalid.ToInt32OrDefault(() => -2); // return -2;

            // Unit Test
            Assert.AreEqual(32, result1);
            Assert.AreEqual(0, result2);
            Assert.AreEqual(-1, result3);
            Assert.AreEqual(-2, result4);
        }
    }
}