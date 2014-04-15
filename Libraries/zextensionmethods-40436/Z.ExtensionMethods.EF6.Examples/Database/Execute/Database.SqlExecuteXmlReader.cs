using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_SqlExecuteXmlReader
    {
        [TestMethod]
        public void ExecuteReader()
        {
            string sql = "SELECT '1' AS A WHERE @Fizz = 1 FOR XML RAW";
            var dict = new Dictionary<string, object> {{"@Fizz", 1}};

            // Examples
            using (var ctx = new EntityFrameworkTestEntities())
            {
                using (XmlReader reader = ctx.Database.SqlExecuteXmlReader(sql, dict.ToSqlParameters()))
                {
                    reader.Read();

                    object result1 = reader[0];

                    // Unit Test
                    Assert.AreEqual("1", result1);
                }
            }
        }
    }
}