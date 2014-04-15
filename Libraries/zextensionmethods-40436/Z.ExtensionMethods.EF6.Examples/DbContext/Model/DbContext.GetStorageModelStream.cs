using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetStorageModelStream
    {
        [TestMethod]
        public void GetStorageModelStream()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                using (Stream stream = ctx.GetStorageModelStream())
                {
                    // Unit Test
                    Assert.IsNotNull(stream);
                }
            }
        }
    }
}