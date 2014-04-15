using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Collections_Generic_IEnumerable_T_StringJoin
    {
        [TestMethod]
        public void StringJoin()
        {
            // Type
            IEnumerable<string> @this = new List<string> {"zA", "zB", "C"}.AsEnumerable();

            // Exemples
            string result = @this.StringJoin(";"); // return "zA;zB;C";

            // Unit Test
            Assert.AreEqual("zA;zB;C", result);
        }
    }
}