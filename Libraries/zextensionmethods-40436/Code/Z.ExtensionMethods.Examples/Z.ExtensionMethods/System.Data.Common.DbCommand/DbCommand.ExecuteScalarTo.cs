using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_Common_DbCommand_ExecuteScalarTo
    {
        [TestMethod]
        public void ExecuteScalarTo()
        {
            const string sql = @"SELECT 1 As IntColumn";

            // Examples
            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                using (SqlCommand @this = conn.CreateCommand())
                {
                    conn.Open();
                    @this.CommandText = sql;
                    var result = @this.ExecuteScalarTo<int>();

                    // UnitTest
                    Assert.AreEqual(1, result);
                }
            }
        }
    }
}