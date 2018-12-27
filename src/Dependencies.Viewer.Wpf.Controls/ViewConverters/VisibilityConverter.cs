using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Dependencies.Viewer.Wpf.Controls.ViewConverters
{
    public class VisibilityConverter : IValueConverter
    {
        public static IValueConverter Converter { get; } = new VisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool isVisible))
                return null;

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
