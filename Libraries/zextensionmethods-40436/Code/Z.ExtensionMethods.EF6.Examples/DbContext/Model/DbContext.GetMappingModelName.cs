using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetMappingModelName
    {
        [TestMethod]
        public void GetMappingModelName()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string result = ctx.GetMappingModelName();

                // Unit Test
                Assert.AreEqual("Model.msl", result);
            }
        }
    }
}