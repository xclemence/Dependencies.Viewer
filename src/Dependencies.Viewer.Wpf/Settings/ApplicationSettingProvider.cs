using Dependencies.Viewer.Wpf.Controls.Base;

namespace Dependencies.Viewer.Wpf.Settings
{
    public class ApplicationSettingProvider : SettingProvider, IApplicationSettingProvider
    {
        private const string SelectedThemeKey = "SelectedTheme";

        public string SelectedTheme
        {
            get => this[SelectedThemeKey];
            set => this[SelectedThemeKey] = value;
        }
    }
}
