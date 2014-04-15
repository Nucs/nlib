using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_GetContext
    {
        [TestMethod]
        public void GetContext()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                var ctx2 = ctx.Database.GetContext<EntityFrameworkTestEntities>();

                // Unit Test
                Assert.AreEqual(ctx, ctx2);
            }
        }
    }
}