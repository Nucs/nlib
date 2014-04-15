using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToEnum
    {
        [TestMethod]
        public void ToEnum()
        {
            // Type
            string @this = "Ordinal";

            // Examples
            var value = @this.ToEnum<StringComparison>(); // return StringComparison.Ordinal;

            // Unit Test
            Assert.AreEqual(StringComparison.Ordinal, value);
        }
    }
}