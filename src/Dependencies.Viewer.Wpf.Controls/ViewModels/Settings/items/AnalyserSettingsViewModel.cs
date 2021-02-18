using System.Collections.Generic;
using Dependencies.Analyser.Base;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Settings
{
    public class AnalyserSettingsViewModel
    {
        public AnalyserSettingsViewModel(AnalyserProvider analyserProvider, IAnalyserSettingProvider settingProvider)
        {
            AnalyserProvider = analyserProvider;
            Settings = settingProvider;
        }

        private AnalyserProvider AnalyserProvider { get; }

        public IAnalyserSettingProvider Settings { get; }

        public IEnumerable<IAssemblyAnalyserFactory> AnalyserFactories => AnalyserProvider.AnalyserFactories;

        public IAssemblyAnalyserFactory? SelectedAnalyserFactory
        {
            get => AnalyserProvider.CurrentAnalyserFactory;
            set
            {
                AnalyserProvider.CurrentAnalyserFactory = value;

                if (value != null)
                    Settings.SaveSetting(SettingKeys.SelectedAnalyserCode, value.Code);
            }
        }
    }
}