using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Net_WebResponse_ReadToEndAndDispose
    {
        [TestMethod]
        public void ReadToEndAndDispose()
        {
            WebRequest webRequest = WebRequest.Create("http://www.zzzportal.com/");

            // Type
            WebResponse @this = webRequest.GetResponseSafe();

            // Examples
            string value = @this.ReadToEndAndDispose();

            // Unit Test
            Assert.IsNotNull(value);
        }
    }
}