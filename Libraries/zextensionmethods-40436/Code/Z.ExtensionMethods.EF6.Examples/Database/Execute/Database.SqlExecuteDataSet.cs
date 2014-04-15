using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_SqlExecuteDataSet
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
            using (var ctx = new EntityFrameworkTestEntities())
            {
                result = ctx.Database.SqlExecuteDataSet(sql, dict.ToSqlParameters()); // return DataSet (3 tables).
            }

            // Unit Test
            Assert.AreEqual(3, result.Tables.Count);
        }
    }
}