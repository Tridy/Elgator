using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Elgator.UI.Converters
{
    public partial class ThumbColorConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Count() != 4)
            {
                return new SolidColorBrush(Colors.Red);
            }

            var brush = values[0] as LinearGradientBrush;

            if (brush == null)
            {
                return new SolidColorBrush(Colors.Red);
            }

            var startColor = brush.GradientStops[0].Color;
            var endColor = brush.GradientStops[1].Color;
            var startValue = (double)values[1];
            var endValue = (double)values[2];
            var value = (double)values[3];

            var color = ColorHelper.GetRelativeColor(startColor, endColor, startValue, endValue, value);

            return new SolidColorBrush(color);
        }
    }
}