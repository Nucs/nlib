using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_SqlExecuteDataTable
    {
        [TestMethod]
        public void ExecuteDataTable()
        {
            DataTable result;
            const string sql = @"
SELECT  1 AS A
UNION
SELECT  2
UNION
SELECT  3 WHERE @Fizz = 1
";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            // Examples
            using (var ctx = new EntityFrameworkTestEntities())
            {
                result = ctx.Database.SqlExecuteDataTable(sql, dict.ToSqlParameters()); // return DataTable (3 rows).
            }

            // Unit Test
            Assert.AreEqual(3, result.Rows.Count);
        }
    }
}