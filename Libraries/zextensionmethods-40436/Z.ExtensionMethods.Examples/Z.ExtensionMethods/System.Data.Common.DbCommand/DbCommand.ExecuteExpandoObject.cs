using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_Common_DbCommand_ExecuteExpandoObject
    {
        [TestMethod]
        public void ExecuteExpandoObject()
        {
            string sql = "SELECT 1 AS IntColumn, 'FizzBuzz' AS StringColumn";

            // Examples
            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                conn.Open();
                using (DbCommand command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    dynamic entity = command.ExecuteExpandoObject();

                    // Unit Test
                    Assert.AreEqual(1, entity.IntColumn);
                    Assert.AreEqual("FizzBuzz", entity.StringColumn);
                }
            }
        }
    }
}