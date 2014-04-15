using System.Data.Entity.Core.EntityClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_GetEntityConnection
    {
        [TestMethod]
        public void GetEntityConnection()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                EntityConnection result = ctx.Database.GetEntityConnection();
                ctx.Clients.AddOrUpdateExtension();
                // UnitTest
                Assert.IsNotNull(result);
            }
        }
    }
}