using System.Linq;
using System.Windows;
using Dragablz;

namespace Dependencies.Viewer.Wpf.Layouts;

public class KeepOneInterLayoutClient : DefaultInterTabClient
{
    public override INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source) => 
        base.GetNewHost(interTabClient, partition, source);


    

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
