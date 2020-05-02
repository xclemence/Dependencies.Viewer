using System.Collections.Generic;
using Dependencies.Analyser.Base;
using Dependencies.Viewer.Wpf.Controls.Fwk;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel(AnalyserProvider analyserProvider, ISettingProvider settingProvider)
        {
            AnalyserProvider = analyserProvider;
            Settings = settingProvider;
        }

        private AnalyserProvider AnalyserProvider { get; }
        public ISettingProvider Settings { get; }

        public IEnumerable<IAssemblyAnalyserFactory> AnalyserFactories => AnalyserProvider.AnalyserFactories;

        public IAssemblyAnalyserFactory SelectedAnalyserFactory
        {
            get => AnalyserProvider.CurrentAnalyserFactory;
            set
            {
                AnalyserProvider.CurrentAnalyserFactory = value;

                if(value != null)
                    Settings.SaveSetting(SettingKeys.SelectedAnalyserCode, value.Code);
            }
        }
    }
}