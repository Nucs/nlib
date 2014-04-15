using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_SqlClient_SqlConnection_ExecuteDataSet
    {
        [TestMethod]
        public void ExecuteDataSet()
        {
            DataSet result;
            const string sql = @"
SELECT 1
SELECT 2
SELECT 3 WHERE @Fizz = 1
";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            // Examples
            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                conn.Open();
                result = conn.ExecuteDataSet(sql, dict.ToSqlParameters()); // return DataSet (3 tables).
            }

            // Unit Test
            Assert.AreEqual(3, result.Tables.Count);
        }
    }
}