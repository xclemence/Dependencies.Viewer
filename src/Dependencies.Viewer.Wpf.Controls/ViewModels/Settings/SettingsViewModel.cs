namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Settings
{
    public class SettingsViewModel
    {
        public SettingsViewModel(AnalyserSettingsViewModel analyserViewModel,
                                 ExchangeSettingsViewModel exchangeViewModel,
                                 ThemeSettingsViewModel themeViewModel)
        {
            AnalyserSettingsViewModel = analyserViewModel;
            ExchangeSettingsViewModel = exchangeViewModel;
            ThemeSettingsViewModel = themeViewModel;
        }

        public AnalyserSettingsViewModel AnalyserSettingsViewModel { get; }
        public ExchangeSettingsViewModel ExchangeSettingsViewModel { get; }
        public ThemeSettingsViewModel ThemeSettingsViewModel { get; }
    }
}