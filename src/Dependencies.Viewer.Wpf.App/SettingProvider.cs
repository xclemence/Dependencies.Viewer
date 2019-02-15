using Dependencies.Viewer.Wpf.Controls;

namespace Dependencies.Viewer.Wpf.App
{
    class SettingProvider : ISettingProvider
    {
        public T GetSettring<T>(string code)
        {
            var item = (T)Properties.Settings.Default[code];
            return item;
        }

        public void SaveSetting<T>(string code, T value)
        {
            Properties.Settings.Default[code] = value;
            Properties.Settings.Default.Save();
        }
    }
}
