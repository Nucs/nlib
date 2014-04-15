using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.EntityFramework.Model;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetModel
    {
        [TestMethod]
        public void GetModel()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                Model model = ctx.GetModel();

                // Unit Test
                Assert.IsNotNull(model);
            }

            using (var ctx = new CodeFirstContext())
            {
                // Examples
                Model model = ctx.GetModel();

                // Unit Test
                Assert.IsNotNull(model);
            }
        }
    }
}