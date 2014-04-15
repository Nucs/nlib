using System.IO;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Web_HttpResponse_Reload
    {
        [TestMethod]
        public void Reload()
        {
            var writer = new StringWriter();

            // Type
            var @this = new HttpResponse(writer);

            // Examples
            try
            {
                @this.Reload();
            }
            catch
            {
            }
        }
    }
}