using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetConceptualModelName
    {
        [TestMethod]
        public void GetConceptualModelName()
        {
            using (var ctx = new EntityFrameworkTestEntities())
            {
                // Examples
                string result = ctx.GetConceptualModelName();

                // Unit Test
                Assert.AreEqual("Model.csdl", result);
            }
        }
    }
}