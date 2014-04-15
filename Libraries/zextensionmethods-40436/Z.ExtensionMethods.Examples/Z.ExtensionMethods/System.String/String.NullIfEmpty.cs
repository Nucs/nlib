using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_NullIfEmpty
    {
        [TestMethod]
        public void NullIfEmpty()
        {
            // Type
            string @this = "";

            // Examples
            string value = @this.NullIfEmpty(); // return null;

            // Unit Test
            Assert.IsNull(value);
        }
    }
}