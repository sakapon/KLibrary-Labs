using System;
using System.Windows.Media;

namespace KLibrary.Labs.UI
{
    /// <summary>
    /// Provides the helper methods for colors.
    /// </summary>
    public static class ColorsHelper
    {
        /// <summary>
        /// Converts a value over [0, 1) to a color for heat maps.
        /// </summary>
        /// <param name="value">A value over [0, 1).</param>
        /// <returns>A color for heat maps (between blue and red). Blue if <paramref name="value"/> &lt; 0; red if <paramref name="value"/> &gt;= 1.</returns>
        /// <remarks>
        /// Color scale: 0000FF - 00FFFF - 00FF00 - FFFF00 - FF0000.
        /// </remarks>
        public static Color ToHeatColor(double value)
        {
            if (value < 0) return Color.FromRgb(0, 0, 255);
            if (value >= 1) return Color.FromRgb(255, 0, 0);

            // [0, 256) の範囲の double 値を扱い、最後に (byte)v で変換します。
            var red
                = value < 0.5 ? 0
                : value < 0.75 ? 256 * 4 * (value - 0.5)
                : 256;
            var green
                = value < 0.25 ? 256 * 4 * value
                : value < 0.75 ? 256
                : 256 * 4 * (1 - value);
            var blue
                = value < 0.25 ? 256
                : value < 0.5 ? 256 * 4 * (0.5 - value)
                : 0;

            return Color.FromRgb(ToByte(red), ToByte(green), ToByte(blue));
        }

        static readonly Func<double, byte> ToByte = v => (byte)(v == 256.0 ? 255.0 : v);
    }
}
