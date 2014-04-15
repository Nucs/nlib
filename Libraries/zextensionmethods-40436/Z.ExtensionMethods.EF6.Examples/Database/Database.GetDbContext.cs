using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_GetDbContext
    {
        [TestMethod]
        public void GetDbContext()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                DbContext ctx2 = ctx.Database.GetDbContext();

                // Unit Test
                Assert.AreEqual(ctx, ctx2);
            }
        }
    }
}