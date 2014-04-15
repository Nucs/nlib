using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetConceptualModelString
    {
        [TestMethod]
        public void GetConceptualModelString()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string s = ctx.GetConceptualModelString();

                // Unit Test
                Assert.IsFalse(string.IsNullOrEmpty(s));
            }
        }
    }
}