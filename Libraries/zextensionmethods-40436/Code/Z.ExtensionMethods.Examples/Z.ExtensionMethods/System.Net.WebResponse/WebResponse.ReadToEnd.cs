using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Net_WebResponse_ReadToEnd
    {
        [TestMethod]
        public void ReadToEnd()
        {
            WebRequest webRequest = WebRequest.Create("http://www.zzzportal.com/");

            // Type
            WebResponse @this = webRequest.GetResponseSafe();

            // Examples
            string value = null;
            using (@this)
            {
                value = @this.ReadToEnd();
            }

            // Unit Test
            Assert.IsNotNull(value);
        }
    }
}