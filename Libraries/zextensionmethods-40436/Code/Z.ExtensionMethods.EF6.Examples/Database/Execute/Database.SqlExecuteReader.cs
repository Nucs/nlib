using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_SqlExecuteReader
    {
        [TestMethod]
        public void ExecuteReader()
        {
            string sql = "SELECT 1 AS IntColumn, 'FizzBuzz' AS StringColumn WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            // Examples
            using (var ctx = new EntityFrameworkTestEntities())
            {
                using (DbConnection conn = ctx.Database.Connection)
                {
                    conn.Open();
                    using (IDataReader reader = ctx.Database.SqlExecuteReader(sql, dict.ToSqlParameters()))
                    {
                        reader.Read();
                        object result1 = reader[0];
                        object result2 = reader[1];

                        // Unit Test
                        Assert.AreEqual(1, result1);
                        Assert.AreEqual("FizzBuzz", result2);
                    }
                }
            }
        }
    }
}