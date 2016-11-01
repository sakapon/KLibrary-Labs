using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KLibrary.Labs.UI.Converters
{
    /// <summary>
    /// Represents the value converter that uses defined functions.
    /// </summary>
    [ValueConversion(typeof(object), typeof(object))]
    public class FuncConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty ToFuncProperty =
            DependencyProperty.Register("ToFunc", typeof(MulticastDelegate), typeof(FuncConverter), new PropertyMetadata(null));

        public static readonly DependencyProperty FromFuncProperty =
            DependencyProperty.Register("FromFunc", typeof(MulticastDelegate), typeof(FuncConverter), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the function to use in the <see cref="Convert"/> method.
        /// </summary>
        public MulticastDelegate ToFunc
        {
            get { return (MulticastDelegate)GetValue(ToFuncProperty); }
            set { SetValue(ToFuncProperty, value); }
        }

        /// <summary>
        /// Gets or sets the function to use in the <see cref="ConvertBack"/> method.
        /// </summary>
        public MulticastDelegate FromFunc
        {
            get { return (MulticastDelegate)GetValue(FromFuncProperty); }
            set { SetValue(FromFuncProperty, value); }
        }

        /// <summary>
        /// Converts a value using the function represented by the <see cref="ToFunc"/> property.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DoFunc(ToFunc, value, parameter);
        }

        /// <summary>
        /// Converts a value using the function represented by the <see cref="FromFunc"/> property.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DoFunc(FromFunc, value, parameter);
        }

        static object DoFunc(MulticastDelegate func, object value, object parameter)
        {
            if (func == null) return value;

            if (func.Method.ContainsGenericParameters) return Binding.DoNothing;

            var parameterInfoes = func.Method.GetParameters();

            switch (parameterInfoes.Length)
            {
                case 0: return func.DynamicInvoke();
                case 1: return func.DynamicInvoke(value);
                case 2: return func.DynamicInvoke(value, parameter);
                default: return Binding.DoNothing;
            }
        }
    }
}
