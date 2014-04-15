using System.IO;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Web_HttpResponse_Redirect
    {
        [TestMethod]
        public void Redirect()
        {
            var writer = new StringWriter();

            // Type
            var @this = new HttpResponse(writer);

            // Examples
            try
            {
                @this.Redirect("http://zzzportal.{0}", "com");
            }
            catch
            {
            }
        }
    }
}