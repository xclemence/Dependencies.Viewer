using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Base;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Settings
{
    public class ThemeSettingsViewModel : ObservableObject
    {
        private readonly ThemeManager themeManager;
        private string selectedTheme;

        public ThemeSettingsViewModel(ThemeManager themeManager, IApplicationSettingProvider settingProvider)
        {
            this.themeManager = themeManager;
            Settings = settingProvider;

            Themes = this.themeManager.Themes.Keys.ToList();

            selectedTheme = settingProvider.SelectedTheme;
        }

        private IApplicationSettingProvider Settings { get; }

        public IList<string> Themes { get; }

        public string SelectedTheme
        {
            get => selectedTheme;
            set
            {
                if (Set(ref selectedTheme, value))
                    UpdateTheme(value);
            }
        }

        private void UpdateTheme(string value) 
        {
            themeManager.ApplyTheme(value);
            Settings.SelectedTheme = value;
        }
    }
}