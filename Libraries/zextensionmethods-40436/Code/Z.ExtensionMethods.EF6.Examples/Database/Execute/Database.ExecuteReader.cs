using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_ExecuteReader
    {
        [TestMethod]
        public void ExecuteReader()
        {
            string sql = "SELECT 1 AS IntColumn, 'FizzBuzz' AS StringColumn WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            using (var ctx = new EntityFrameworkTestEntities())
            {
                using (DbConnection conn = ctx.Database.Connection)
                {
                    conn.Open();

                    // Examples
                    using (IDataReader reader = ctx.Database.ExecuteReader(sql, dict.ToDbParameters(ctx.Database.Connection)))
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