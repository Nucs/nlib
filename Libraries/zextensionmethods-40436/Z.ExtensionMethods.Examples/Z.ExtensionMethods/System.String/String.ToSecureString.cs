using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_String_ToSecureString
    {
        [TestMethod]
        public void ToSecureString()
        {
            // Type
            string @this = "FizzBuzz";

            // Exemples
            SecureString result = @this.ToSecureString();
        }
    }
}