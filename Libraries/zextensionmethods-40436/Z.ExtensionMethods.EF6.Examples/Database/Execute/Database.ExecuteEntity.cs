using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_ExecuteEntity
    {
        [TestMethod]
        public void ExecuteEntity()
        {
            string sql = "SELECT 1 AS IntColumn, 'FizzBuzz' AS StringColumn WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples

                var entity = ctx.Database.ExecuteEntity<TestObject>(sql, dict.ToDbParameters(ctx.Database.Connection));

                // Unit Test
                Assert.AreEqual(1, entity.IntColumn);
                Assert.AreEqual("FizzBuzz", entity.StringColumn);
                Assert.AreEqual(-1, entity.IntColumnNotExists);
            }
        }

        public class TestObject
        {
            public int IntColumn;
            public int IntColumnNotExists = -1;
            public string StringColumnNotExists;
            public string StringColumn { get; set; }
        }
    }
}