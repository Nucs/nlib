using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetMappingModelStream
    {
        [TestMethod]
        public void GetMappingModelStream()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                using (Stream stream = ctx.GetMappingModelStream())
                {
                    // Unit Test
                    Assert.IsNotNull(stream);
                }
            }
        }
    }
}