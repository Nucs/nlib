using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;
using Z.ExtensionMethods.Object;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_SqlBulKTestTransaction
    {
        [TestMethod]
        public void SqlBulKTestTransaction()
        {
            string sql = @"
TRUNCATE TABLE TestWithoutIdentityCustomName
INSERT INTO TestWithoutIdentityCustomName VALUES (1, 1)
INSERT INTO TestWithoutIdentityCustomName VALUES (2, 2)
";
            string sqlCount = "SELECT SUM(CustomIntValue) FROM TestWithoutIdentityCustomName";

            using (var ctx = new CodeFirstContext())
            {
                ctx.Database.ExecuteNonQuery(sql);
                Assert.AreEqual(3, ctx.Database.ExecuteScalar(sqlCount).To<int>());
            }

            using (var transaction = new TransactionScope())
            {
                using (var ctx = new CodeFirstContext())
                {
                    List<TestWithoutIdentity> list = ctx.TestWithoutIdentitys.ToList();
                    list[0].IntValue = 3;
                    list[1].IntValue = 4;
                    list.Add(new TestWithoutIdentity {ID = 3, IntValue = 5});

                    ctx.SqlBulkMerge(list);
                }

                //transaction.Complete();
            }

            using (var ctx = new CodeFirstContext())
            {
                Assert.AreEqual(3, ctx.Database.ExecuteScalar(sqlCount).To<int>());
            }

            using (var ctx = new CodeFirstContext())
            {
                using (DbContextTransaction transaction = ctx.Database.BeginTransaction())
                {
                    List<TestWithoutIdentity> list = ctx.TestWithoutIdentitys.ToList();
                    list[0].IntValue = 3;
                    list[1].IntValue = 4;
                    list.Add(new TestWithoutIdentity {ID = 3, IntValue = 5});

                    ctx.SqlBulkMerge(list, transaction);

                    transaction.Rollback();
                    //transaction.Commit();
                }
            }


            using (var ctx = new CodeFirstContext())
            {
                Assert.AreEqual(3, ctx.Database.ExecuteScalar(sqlCount).To<int>());
            }
        }
    }
}