using System.Data.Common;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_GetDbTransaction
    {
        [TestMethod]
        public void GetDbTransaction()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                ctx.Database.Connection.Open();
                using (DbTransaction trans = ctx.Database.Connection.BeginTransaction())
                {
                    ctx.Database.UseTransaction(trans);

                    // Examples
                    DbTransaction result = ctx.Database.GetDbTransaction();

                    // Unit Test
                    Assert.AreEqual(trans, result);
                }
            }

            using (var ctx = new EntityFrameworkTestEntities())
            {
                using (DbContextTransaction trans = ctx.Database.BeginTransaction())
                {
                    // Examples
                    DbTransaction result = ctx.Database.GetDbTransaction();

                    // Unit Test
                    Assert.IsNotNull(result);
                }
            }
        }
    }
}