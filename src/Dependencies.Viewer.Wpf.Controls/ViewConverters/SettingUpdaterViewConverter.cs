using System;
using System.Globalization;
using System.Windows.Data;
using Dependencies.Exchange.Base;

namespace Dependencies.Viewer.Wpf.Controls.ViewConverters
{
    public class SettingUpdaterViewConverter : IValueConverter
    {
        public static IValueConverter Converter { get; } = new SettingUpdaterViewConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ISettingUpdaterProvider settingUpdaterProvider) return null;

            return settingUpdaterProvider.GetSettingView();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
