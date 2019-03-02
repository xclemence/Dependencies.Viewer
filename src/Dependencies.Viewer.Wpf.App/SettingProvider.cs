using Dependencies.Analyser.Base;

namespace Dependencies.Viewer.Wpf.App
{
    class SettingProvider : ISettingProvider
    {
        public dynamic this[string code]
        {
            get => GetSettring<dynamic>(code);
            set => SaveSetting<dynamic>(code, value);
        }

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
