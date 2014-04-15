using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_SqlBulkDelete
    {
        [TestMethod]
        public void SqlBulkDelete()
        {
            // Database First
            System_Data_SqlClient_SqlBulkCopy_TestHelper.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelper.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());

            using (var ctx = new EntityFrameworkTestEntities())
            {
                List<BulkCopyTest2> bulkCopyTest = ctx.BulkCopyTest2.ToList();
                ctx.SqlBulkDelete(bulkCopyTest);
                Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelper.ItemCount());
            }

            // Code First
            System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.InitializeDB();
            System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.CleanData();
            System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.Insert(100);
            Assert.AreEqual(100, System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.ItemCount());

            using (var ctx = new CodeFirstContext())
            {
                List<BulkCopyTestCodeFirst> bulkCopyTest = ctx.BulkCopyTestCodeFirsts.ToList();
                ctx.SqlBulkDelete(bulkCopyTest);
                Assert.AreEqual(0, System_Data_SqlClient_SqlBulkCopy_TestHelperCodeFirst.ItemCount());
            }
        }
    }
}