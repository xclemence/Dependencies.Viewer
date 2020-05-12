using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Dependencies.Analyser.Base;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Fwk;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private UserControl selectedExchangeSettingsControl;
        private ISettingUpdaterProvider selectedSettingUpdaterProvider;

        public SettingsViewModel(AnalyserProvider analyserProvider,
                                 ISettingProvider settingProvider,
                                 IEnumerable<ISettingUpdaterProvider> settingUpdaterProviders)
        {
            AnalyserProvider = analyserProvider;
            Settings = settingProvider;
            SettingUpdaterProviders = settingUpdaterProviders.ToList();
            SelectedSettingUpdaterProvider = SettingUpdaterProviders.FirstOrDefault();
        }

        private AnalyserProvider AnalyserProvider { get; }
        public ISettingProvider Settings { get; }

        public IEnumerable<IAssemblyAnalyserFactory> AnalyserFactories => AnalyserProvider.AnalyserFactories;
        public IList<ISettingUpdaterProvider> SettingUpdaterProviders { get; }

        public IAssemblyAnalyserFactory SelectedAnalyserFactory
        {
            get => AnalyserProvider.CurrentAnalyserFactory;
            set
            {
                AnalyserProvider.CurrentAnalyserFactory = value;

                if (value != null)
                    Settings.SaveSetting(SettingKeys.SelectedAnalyserCode, value.Code);
            }
        }

        public ISettingUpdaterProvider SelectedSettingUpdaterProvider
        {
            get => selectedSettingUpdaterProvider;
            set
            {
                selectedSettingUpdaterProvider = value;
                SelectedExchangeSettingsControl = selectedSettingUpdaterProvider?.GetSettingView();
            }
        }

        public UserControl SelectedExchangeSettingsControl
        {
            get => selectedExchangeSettingsControl;
            set => Set(ref selectedExchangeSettingsControl, value);
        }
    }
}