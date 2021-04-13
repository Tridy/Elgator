using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Elgator.UI.Converters
{
    public class ThumbColorConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values == null || values.Count() != 4)
            {
                return new SolidColorBrush(Colors.Red);
            }

            var brush = values[0] as LinearGradientBrush;

            if(brush == null)
            {
                return new SolidColorBrush(Colors.Red);
            }

            var startColor = brush.GradientStops[0].Color;
            var endColor = brush.GradientStops[1].Color;
            var startValue = (double)values[1];
            var endValue = (double)values[2];
            var value = (double)values[3];

            var color = GetRelativeColor(startColor, endColor, startValue, endValue, value);

            return new SolidColorBrush(color);
        }

        public Color GetRelativeColor(Color startColor, Color endColor, double startValue, double endValue, double value)
        {
            double numA = (value - startValue) * (endColor.A - startColor.A) / (endValue - startValue);
            Color c = startColor;
            double a = c.A;
            int num2 = (byte)(numA + a);
            var A = (byte)num2;

            double numR = value - startValue;
            c = endColor;
            int r1 = c.R;
            c = startColor;
            int r2 = c.R;
            double num4 = r1 - r2;
            double ratioR = numR * num4 / (endValue - startValue);
            c = startColor;
            double r3 = c.R;
            int num6 = (byte)(ratioR + r3);
            var R = (byte)num6;

            double num7 = value - startValue;
            c = endColor;
            int g1 = c.G;
            c = startColor;
            int g2 = c.G;
            double num8 = g1 - g2;
            double ratioG = num7 * num8 / (endValue - startValue);
            c = startColor;
            double g3 = c.G;
            int numG = (byte)(ratioG + g3);
            var G = (byte)numG;

            double num11 = value - startValue;
            c = endColor;
            int b1 = c.B;
            c = startColor;
            int b2 = c.B;
            double bDiff = b1 - b2;
            double ratioB = num11 * bDiff / (endValue - startValue);
            c = startColor;
            double b3 = c.B;
            int numB = (byte)(ratioB + b3);
            var B = (byte)numB;

            var color = Color.FromArgb(A, R, G, B);

            return color;
        }
    }
}