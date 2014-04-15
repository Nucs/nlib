using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Enum_In
    {
        [TestMethod]
        public void In()
        {
            // Type
            var @this = ConnectionState.Open;

            // Exemples
            bool result1 = @this.In(ConnectionState.Open, ConnectionState.Fetching); // return true;
            bool result2 = @this.In(ConnectionState.Fetching); // return false;

            // Unit Test
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
    }
}