using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_Common_DbConnection_ExecuteNonQuery
    {
        [TestMethod]
        public void ExecuteNonQuery()
        {
            string sql = "DECLARE @FizzBuzz VARCHAR(MAX) = @Fizz";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            // Examples
            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                conn.Open();
                conn.ExecuteNonQuery(sql, dict.ToDbParameters(conn));
            }
        }
    }
}