using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_SqlClient_SqlBulkCopy_GetSqlConnection
    {
        [TestMethod]
        public void GetSqlConnection()
        {
            // Examples
            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (var @this = new SqlBulkCopy(conn))
                {
                    SqlConnection result = @this.GetSqlConnection();

                    // Unit Test
                    Assert.AreEqual(conn, result);
                }
            }
        }
    }
}