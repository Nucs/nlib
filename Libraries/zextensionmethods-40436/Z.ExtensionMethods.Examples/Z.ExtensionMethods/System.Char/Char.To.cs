using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Char_To
    {
        [TestMethod]
        public void To()
        {
            // Type
            char @this = 'A';

            // Examples
            List<char> list = @this.To('C').ToList(); // return new [] {'a', 'b', 'c'};

            // Unit Test
            Assert.AreEqual(3, list.Count);
        }
    }
}