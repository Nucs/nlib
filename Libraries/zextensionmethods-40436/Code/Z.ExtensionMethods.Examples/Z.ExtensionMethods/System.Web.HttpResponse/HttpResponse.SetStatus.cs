using System.IO;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Web_HttpResponse_SetStatus
    {
        [TestMethod]
        public void SetStatus()
        {
            var writer = new StringWriter();

            // Type
            var @this = new HttpResponse(writer);

            // Examples
            @this.SetStatus(200, "FizzBuzz", false);

            // UnitTest
            Assert.AreEqual(200, @this.StatusCode);
            Assert.AreEqual("FizzBuzz", @this.StatusDescription);
        }
    }
}