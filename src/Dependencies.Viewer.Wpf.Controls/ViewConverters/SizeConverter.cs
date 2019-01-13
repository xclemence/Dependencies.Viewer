using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Dependencies.Viewer.Wpf.Controls.ViewConverters
{
    public class SizeConverter : IMultiValueConverter
    {
        public static IMultiValueConverter Converter { get; } = new SizeConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var size = (double)values[0];

            foreach (var item in values.Skip(1))
                size -= double.Parse(item.ToString());

            if (size <= 0)
                return 0.1;

            return size;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
