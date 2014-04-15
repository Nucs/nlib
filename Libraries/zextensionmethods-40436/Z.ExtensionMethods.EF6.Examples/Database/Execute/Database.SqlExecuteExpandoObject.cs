using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_SqlExecuteExpandoObject
    {
        [TestMethod]
        public void ExecuteExpandoObject()
        {
            string sql = "SELECT 1 AS IntColumn, 'FizzBuzz' AS StringColumn WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            // Examples
            using (var ctx = new EntityFrameworkTestEntities())
            {
                dynamic entity = ctx.Database.SqlExecuteExpandoObject(sql, dict.ToSqlParameters());

                // Unit Test
                Assert.AreEqual(1, entity.IntColumn);
                Assert.AreEqual("FizzBuzz", entity.StringColumn);
            }
        }
    }
}