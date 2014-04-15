using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Net_WebRequest_GetResponseSafe
    {
        [TestMethod]
        public void GetResponseSafe()
        {
            // Type
            WebRequest @this = WebRequest.Create("http://www.zzzportal.com/");

            // Examples
            WebResponse value = @this.GetResponseSafe(); // return a response;

            // Unit Test
            Assert.IsNotNull(value);
        }
    }
}