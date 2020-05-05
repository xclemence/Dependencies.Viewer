using Dependencies.Exchange.Base;
using Dependencies.Exchange.Graph.Settings;

namespace Dependencies.Exchange.Graph.ViewModels
{
    public class GraphSettingsViewModel
    {
        private readonly GraphSettings settings;
        private readonly ISettingServices<GraphSettings> settingServices;

        public GraphSettingsViewModel(ISettingServices<GraphSettings> settingServices)
        {
            this.settingServices = settingServices;

            settings = settingServices.GetSettings();
        }

        public string Uri
        {
            get => settings.ServiceUri;
            set 
            {
                settings.ServiceUri = value;
                settingServices.SaveSettings(settings);
            }
        }
    }
}
