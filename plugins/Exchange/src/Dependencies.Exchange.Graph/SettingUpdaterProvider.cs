using System.Windows.Controls;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Graph.Settings;
using Dependencies.Exchange.Graph.ViewModels;
using Dependencies.Exchange.Graph.Views;

namespace Dependencies.Exchange.Graph
{
    public class SettingUpdaterProvider : ISettingUpdaterProvider
    {
        private readonly ISettingServices<GraphSettings> settingServices;

        public SettingUpdaterProvider(ISettingServices<GraphSettings> settingServices)
        {
            this.settingServices = settingServices;
        }

        public string Name => "Graph Services";

        public UserControl GetSettingView() => new GraphSettingsView
        {
            DataContext = new GraphSettingsViewModel(settingServices)
        };
    }
}
