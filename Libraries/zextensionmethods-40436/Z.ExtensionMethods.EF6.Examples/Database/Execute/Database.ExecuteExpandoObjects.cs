using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_ExecuteExpandoObjects
    {
        [TestMethod]
        public void ExecuteExpandoObjects()
        {
            string sql = "SELECT 1 AS IntColumn, 'FizzBuzz' AS StringColumn UNION SELECT 2, 'BuzzBuzz' WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                List<dynamic> list = ctx.Database.ExecuteExpandoObjects(sql, dict.ToDbParameters(ctx.Database.Connection)).ToList();

                // Unit Test
                Assert.AreEqual(1, list[0].IntColumn);
                Assert.AreEqual("FizzBuzz", list[0].StringColumn);
                Assert.AreEqual(2, list[1].IntColumn);
                Assert.AreEqual("BuzzBuzz", list[1].StringColumn);
            }
        }
    }
}