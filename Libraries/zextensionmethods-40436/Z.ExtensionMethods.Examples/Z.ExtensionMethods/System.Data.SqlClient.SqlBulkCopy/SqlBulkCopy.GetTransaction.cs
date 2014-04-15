using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_SqlClient_SqlBulkCopy_GetTransaction
    {
        [TestMethod]
        public void GetTransaction()
        {
            // Examples
            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                using (var @this = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                {
                    SqlTransaction result = @this.GetTransaction();

                    // Unit Test
                    Assert.AreEqual(trans, result);
                }
            }
        }
    }
}