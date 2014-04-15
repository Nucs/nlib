using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetTableName
    {
        [TestMethod]
        public void GetTableName()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string result = ctx.GetTableName<Client>();

                // UnitTest
                Assert.AreEqual("[dbo].[Client]", result);
            }
        }
    }
}