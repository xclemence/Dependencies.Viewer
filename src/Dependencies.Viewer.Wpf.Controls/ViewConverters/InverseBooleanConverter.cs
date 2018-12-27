using System;
using System.Globalization;
using System.Windows.Data;

namespace Dependencies.Viewer.Wpf.Controls.ViewConverters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public static IValueConverter Converter { get; } = new InverseBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool booleanValue))
                return false;

            return !booleanValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool booleanValue))
                return false;

            return !booleanValue;
        }
    }
}
