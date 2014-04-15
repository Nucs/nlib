using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Windows_Forms_Form_ToFullScreen
    {
        [TestMethod]
        public void ToFullScreen()
        {
            // Type
            var @this = new Form();

            // Examples
            @this.ToFullScreen(); // The Form is now in FullScreen
        }
    }
}