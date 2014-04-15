using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Enum_NotIn
    {
        [TestMethod]
        public void NotIn()
        {
            // Type
            var @this = ConnectionState.Open;

            // Exemples
            bool result1 = @this.NotIn(ConnectionState.Open, ConnectionState.Fetching); // return false;
            bool result2 = @this.NotIn(ConnectionState.Fetching); // return true;

            // Unit Test
            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
        }
    }
}