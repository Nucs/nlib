using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetStorageModelString
    {
        [TestMethod]
        public void GetStorageModelString()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string s = ctx.GetStorageModelString();

                // Unit Test
                Assert.IsFalse(string.IsNullOrEmpty(s));
            }
        }
    }
}