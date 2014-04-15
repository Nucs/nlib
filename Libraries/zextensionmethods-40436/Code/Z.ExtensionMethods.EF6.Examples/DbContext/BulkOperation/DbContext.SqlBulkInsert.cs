using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_SqlBulkInsert
    {
        [TestMethod]
        public void SqlBulkInsert()
        {
            {
                // Database First
                System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
                Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                List<BulkCopyTest2> list = System_Data_SqlClient_SqlBulkCopy_TestHelper.InsertToEntities(100);
                using (var ctx = new EntityFrameworkTestEntities())
                {
                    ctx.SqlBulkInsert(list);
                    Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
                }
            }

            {
                // Code First
                System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.InitializeDB();
                System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.CleanData();
                Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.ItemCount());
                List<BulkCopyTestCodeFirst> list = System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.InsertToEntities(100);

                using (var ctx = new CodeFirstContext())
                {
                    ctx.SqlBulkInsert(list);
                    Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.ItemCount());
                }
            }
        }
    }
}