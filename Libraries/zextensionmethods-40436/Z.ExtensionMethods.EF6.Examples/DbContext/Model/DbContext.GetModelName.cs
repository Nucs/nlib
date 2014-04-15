using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetModelName
    {
        [TestMethod]
        public void GetModelName()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string result = ctx.GetModelName();

                // Unit Test
                Assert.AreEqual("Model", result);
            }
        }
    }
}