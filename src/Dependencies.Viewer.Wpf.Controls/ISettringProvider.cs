using Dependencies.Analyser.Base;

namespace Dependencies.Viewer.Wpf.Controls
{
    internal class SettingKeys
    {
        public static string SelectedAnalyserCode => "SelectedAnalyserCode";
    }

    public interface ISettingProvider
    {
        void SaveSetting<T>(string code, T value);

        T GetSettring<T>(string code);
    }
}
