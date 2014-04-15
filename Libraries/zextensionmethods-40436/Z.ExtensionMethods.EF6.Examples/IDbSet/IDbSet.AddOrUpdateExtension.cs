using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class IDbSet_AddOrUpdateExtension
    {
        [TestMethod]
        public void AddOrUpdateExtension()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                var client = new Client {Name = "Fizz"};

                // Examples - Insert
                ctx.Clients.AddOrUpdateExtension(client);
                ctx.SaveChanges();

                // Example - Update
                client.Name = "Buzz";
                ctx.Clients.AddOrUpdateExtension(client);
                ctx.SaveChanges();
            }
        }
    }
}