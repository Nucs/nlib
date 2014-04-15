using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetStorageModelName
    {
        [TestMethod]
        public void GetStorageModelName()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string result = ctx.GetStorageModelName();

                // Unit Test
                Assert.AreEqual("Model.ssdl", result);
            }
        }
    }
}