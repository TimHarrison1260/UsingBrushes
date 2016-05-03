using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace UsingBrushes.Converters
{
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value==null) return new SolidColorBrush();
            var colour = (Color) value;
            var brush = new SolidColorBrush(colour);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //  Would extract the color from the brush and return that.
            //  Don't need that here so we'll not bother doing it.
            throw new NotImplementedException();
        }
    }
}
