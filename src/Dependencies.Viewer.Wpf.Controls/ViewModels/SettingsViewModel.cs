using System.Collections.Generic;
using Dependencies.Analyser.Base;
using GalaSoft.MvvmLight;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingProvider settingProvider;
        private const string SelectedAnalyserCode = "SelectedAnalyserCode";

        public SettingsViewModel(AnalyserProvider analyserProvider, ISettingProvider settingProvider)
        {
            AnalyserProvider = analyserProvider;
            this.settingProvider = settingProvider;
        }

        private AnalyserProvider AnalyserProvider { get; }

        public IEnumerable<IAssemblyAnalyserFactory> AnalyserFactories => AnalyserProvider.AnalyserFactories;

        public IAssemblyAnalyserFactory SelectedAnalyserFactory
        {
            get => AnalyserProvider.CurrentAnalyserFactory;
            set
            {
                AnalyserProvider.CurrentAnalyserFactory = value;

                if(value != null)
                    settingProvider.SaveSetting(SettingKeys.SelectedAnalyserCode, value.Code);
            }
        }
    }
}