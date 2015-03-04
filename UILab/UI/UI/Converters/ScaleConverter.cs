using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KLibrary.Labs.UI.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class ScaleConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(ScaleConverter), new PropertyMetadata(1.0));

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueD = ToValue(value);

            return valueD.HasValue
                ? valueD * Scale
                : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueD = ToValue(value);

            return valueD.HasValue
                ? valueD / Scale
                : DependencyProperty.UnsetValue;
        }

        static double? ToValue(object o)
        {
            try
            {
                return System.Convert.ToDouble(o);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
