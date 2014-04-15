using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_SqlBulkUpdate
    {
        [TestMethod]
        public void SqlBulkUpdate()
        {
            {
                // Database First
                System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
                System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
                Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());

                using (var ctx = new EntityFrameworkTestEntities())
                {
                    List<BulkCopyTest2> bulkCopyTest = ctx.BulkCopyTest2.ToList();
                    foreach (BulkCopyTest2 item in bulkCopyTest)
                    {
                        item.ValueInt = item.ValueInt*2;
                    }
                    ctx.SqlBulkUpdate(bulkCopyTest);
                    Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelper.IntColumnSum());
                }
            }

            {
                // Code First
                System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.InitializeDB();
                System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.CleanData();
                System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.Insert(100);
                Assert.AreEqual(4950, System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.IntColumnSum());

                using (var ctx = new CodeFirstContext())
                {
                    List<BulkCopyTestCodeFirst> bulkCopyTest = ctx.BulkCopyTestCodeFirsts.ToList();
                    foreach (BulkCopyTestCodeFirst item in bulkCopyTest)
                    {
                        item.ValueInt = item.ValueInt*2;
                    }
                    ctx.SqlBulkUpdate(bulkCopyTest);
                    Assert.AreEqual(9900, System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.IntColumnSum());
                }
            }
        }
    }
}