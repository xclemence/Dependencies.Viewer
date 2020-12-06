using System.Diagnostics.CodeAnalysis;

namespace Dependencies.Viewer.Wpf.Settings
{
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Mask usage of application properties")]
    public class SettingProvider
    {
        public dynamic this[string code]
        {
            get => GetSetting<dynamic>(code);
            set => SaveSetting<dynamic>(code, value);
        }

        public T GetSetting<T>(string code)
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
