using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.ViewModels;

namespace Dependencies.Viewer.Wpf.Controls.ViewConverters
{
    public class AnalyseResultViewModelFactoryConverter : IMultiValueConverter
    {
        public static IMultiValueConverter Converter { get; } = new AnalyseResultViewModelFactoryConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var control = values[0] as DependencyObject;

            var assembly = values[1] as AssemblyModel;
            
            if (Window.GetWindow(control) is IFactoryHolder factoryHolder)
            {
                var newViewModel = factoryHolder.ServiceFactory.Create<AnalyseResultViewModel>();
                newViewModel.AssemblyResult = assembly;

                return newViewModel;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
