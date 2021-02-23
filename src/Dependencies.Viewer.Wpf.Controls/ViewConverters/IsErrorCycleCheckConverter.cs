using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Dependencies.Viewer.Wpf.Controls.ViewConverters
{
    public class IsErrorCycleCheckConverter : IMultiValueConverter
    {
        public static IMultiValueConverter Converter { get; } = new IsErrorCycleCheckConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var currentValue = values[0].ToString();

            var list = values[1] as IImmutableList<string>;

            return currentValue == list?[^1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
