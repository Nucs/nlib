using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_GetEntityTransaction
    {
        [TestMethod]
        public void GetEntityTransaction()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                using (DbContextTransaction trans = ctx.Database.BeginTransaction())
                {
                    // Examples
                    EntityTransaction result = ctx.Database.GetEntityTransaction();

                    // Unit Test
                    Assert.IsNotNull(result);
                }
            }
        }
    }
}