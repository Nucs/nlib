using System.Windows.Forms.Design;

namespace nucs.WinForms.WindowFinder
{
    public class WindowFinderDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return (SelectionRules.Moveable | SelectionRules.Visible);
            }
        }
    }
}
