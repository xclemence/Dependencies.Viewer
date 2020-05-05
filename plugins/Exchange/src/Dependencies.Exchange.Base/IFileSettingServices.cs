namespace Dependencies.Exchange.Base
{
    public interface ISettingServices<TSettings>
    {
        TSettings GetSettings();
        void SaveSettings(TSettings settings);
    }
}