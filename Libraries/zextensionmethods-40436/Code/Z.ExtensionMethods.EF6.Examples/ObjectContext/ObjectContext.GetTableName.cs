using System.Data.Entity.Core.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class ObjectContext_GetTableName
    {
        [TestMethod]
        public void GetTableName()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                ObjectContext objectContext = ctx.GetObjectContext();

                // Examples
                string result = objectContext.GetTableName<Client>();

                // UnitTest
                Assert.AreEqual("[dbo].[Client]", result);
            }
        }
    }
}