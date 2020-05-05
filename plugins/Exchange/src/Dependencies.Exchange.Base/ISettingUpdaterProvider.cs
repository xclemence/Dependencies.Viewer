using System.Windows.Controls;

namespace Dependencies.Exchange.Base
{
    public interface ISettingUpdaterProvider
    {
        string Name { get; }
        UserControl GetSettingView();
    }
}