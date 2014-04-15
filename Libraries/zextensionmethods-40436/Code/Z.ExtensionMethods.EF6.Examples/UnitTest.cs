using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Z.ExtensionMethods.EntityFramework.Test
{
    // Keep it for testing purspose
}

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test()
        {
            using (var ctx = new CodeFirstContext())
            {
                var list = ctx.ComplexTypes.ToList();
            }
        }
    }
}