using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dependencies.Viewer.Wpf.Controls.ViewConverters
{
    public class VisibilityConverter : IValueConverter
    {
        public static IValueConverter Converter { get; } = new VisibilityConverter();

        [SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "bad type pattern syntax interpretation")]
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
