using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_GetSqlTransaction
    {
        [TestMethod]
        public void GetSqlTransaction()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                ctx.Database.Connection.Open();
                using (DbTransaction trans = ctx.Database.Connection.BeginTransaction())
                {
                    ctx.Database.UseTransaction(trans);

                    // Examples
                    SqlTransaction result = ctx.Database.GetSqlTransaction();

                    // Unit Test
                    Assert.AreEqual(trans, result);
                }
            }

            using (var ctx = new EntityFrameworkTestEntities())
            {
                using (DbContextTransaction trans = ctx.Database.BeginTransaction())
                {
                    // Examples
                    SqlTransaction result = ctx.Database.GetSqlTransaction();

                    // Unit Test
                    Assert.IsNotNull(result);
                }
            }
        }
    }
}