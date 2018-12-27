using System.Linq;
using System.Windows;
using Dragablz;

namespace Dependencies.Viewer.Wpf.App.Layouts
{
    public class KeepOneInterLayoutClient : DefaultInterTabClient
    {
        public override TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            if (Application.Current.Windows.OfType<MainWindow>().Count() > 1)
            {
                return TabEmptiedResponse.CloseWindowOrLayoutBranch;
            }
            else
            {
                return TabEmptiedResponse.DoNothing;
            }
        }
    }
}
