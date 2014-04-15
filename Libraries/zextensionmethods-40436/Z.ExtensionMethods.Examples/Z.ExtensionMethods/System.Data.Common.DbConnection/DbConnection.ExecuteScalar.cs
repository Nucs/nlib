using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_Common_DbConnection_ExecuteScalar
    {
        [TestMethod]
        public void ExecuteScalar()
        {
            const string sql = @"SELECT 1 As IntColumn WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            // Examples
            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                conn.Open();
                object result = conn.ExecuteScalar(sql, dict.ToDbParameters(conn));

                // UnitTest
                Assert.AreEqual(1, result);
            }
        }
    }
}