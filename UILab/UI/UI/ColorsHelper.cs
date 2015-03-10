using System;
using System.Windows.Media;

namespace KLibrary.Labs.UI
{
    public static class ColorsHelper
    {
        // 数値をヒートマップ用の色に変換します。
        // 数値の範囲は、0 以上 maxValue 以下です。
        // 色は、0000FF - 00FFFF- 00FF00 - FFFF00 - FF0000 の間の連続値で表されます。
        public static Color ToHeatColor(double value, double maxValue)
        {
            if (value < 0) return Color.FromRgb(0, 0, 255);
            if (value > maxValue) return Color.FromRgb(255, 0, 0);

            var red =
                value < maxValue / 2 ? 0 :
                value < 3 * maxValue / 4 ? 255 * 4 * (value - maxValue / 2) / maxValue :
                255;
            var green =
                value < maxValue / 4 ? 255 * 4 * value / maxValue :
                value < 3 * maxValue / 4 ? 255 :
                255 * 4 * (maxValue - value) / maxValue;
            var blue =
                value < maxValue / 4 ? 255 :
                value < maxValue / 2 ? 255 * 4 * (maxValue / 2 - value) / maxValue :
                0;
            return Color.FromRgb(RoundToByte(red), RoundToByte(green), RoundToByte(blue));
        }

        static readonly Func<double, byte> RoundToByte = v => (byte)Math.Round(v, MidpointRounding.AwayFromZero);
    }
}
