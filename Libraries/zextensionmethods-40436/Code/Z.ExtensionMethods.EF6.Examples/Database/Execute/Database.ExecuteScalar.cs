using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_ExecuteScalar
    {
        [TestMethod]
        public void ExecuteScalar()
        {
            const string sql = @"SELECT 1 As IntColumn WHERE @Fizz = 1";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                object result = ctx.Database.ExecuteScalar(sql, dict.ToDbParameters(ctx.Database.Connection));

                // UnitTest
                Assert.AreEqual(1, result);
            }
        }
    }
}