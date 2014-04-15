using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_ExecuteExpandoObject
    {
        [TestMethod]
        public void ExecuteExpandoObject()
        {
            string sql = "SELECT 1 AS IntColumn, 'FizzBuzz' AS StringColumn WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                dynamic entity = ctx.Database.ExecuteExpandoObject(sql, dict.ToDbParameters(ctx.Database.Connection));

                // Unit Test
                Assert.AreEqual(1, entity.IntColumn);
                Assert.AreEqual("FizzBuzz", entity.StringColumn);
            }
        }
    }
}