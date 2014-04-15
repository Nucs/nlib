using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_GetSqlConnection
    {
        [TestMethod]
        public void GetSqlConnection()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                SqlConnection result = ctx.Database.GetSqlConnection();

                // UnitTest
                Assert.AreEqual(ctx.Database.Connection.ConnectionString, result.ConnectionString);
            }
        }
    }
}