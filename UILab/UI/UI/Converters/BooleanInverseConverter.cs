using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KLibrary.Labs.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Consider bool and bool?.
            return value is bool ? !(bool)value
                : value == null ? true
                : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool ? !(bool)value
                : value == null ? true
                : DependencyProperty.UnsetValue;
        }
    }
}
