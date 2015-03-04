using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace KLibrary.Labs.UI.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Color
                ? new SolidColorBrush((Color)value)
                : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is SolidColorBrush
                ? ((SolidColorBrush)value).Color
                : DependencyProperty.UnsetValue;
        }
    }
}
