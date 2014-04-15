using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetConceptualModelStream
    {
        [TestMethod]
        public void GetConceptualModelStream()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                using (Stream stream = ctx.GetConceptualModelStream())
                {
                    // Unit Test
                    Assert.IsNotNull(stream);
                }
            }
        }
    }
}