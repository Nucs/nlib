using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;
using Z.ExtensionMethods.Object;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_SqlBulKTestWithoutIdentity
    {
        [TestMethod]
        public void SqlBulKTestWithoutIdentity()
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

                List<TestWithoutIdentity> list = ctx.TestWithoutIdentitys.ToList();
                list[0].IntValue = 3;
                list[1].IntValue = 4;
                list.Add(new TestWithoutIdentity() {ID = 3, IntValue = 5});

                ctx.SqlBulkMerge(list);

                Assert.AreEqual(12, ctx.Database.ExecuteScalar(sqlCount).To<int>());
            }
        }
    }
}