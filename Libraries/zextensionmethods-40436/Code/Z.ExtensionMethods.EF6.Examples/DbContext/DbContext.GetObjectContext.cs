using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetObjectContext
    {
        [TestMethod]
        public void GetObjectContext()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                ObjectContext result = ctx.GetObjectContext();

                // Unit Test
                Assert.AreEqual(((IObjectContextAdapter) ctx).ObjectContext, result);
            }
        }
    }
}