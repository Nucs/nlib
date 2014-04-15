using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetMappingModelString
    {
        [TestMethod]
        public void GetMappingModelString()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string s = ctx.GetMappingModelString();

                // Unit Test
                Assert.IsFalse(string.IsNullOrEmpty(s));
            }
        }
    }
}