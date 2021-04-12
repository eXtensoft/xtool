using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace XTool
{
    public class BooleanToFontWeightConverter : IValueConverter
    {
        public FontWeight IsFalseWeight { get; set; }

        public FontWeight IsTrueWeight { get; set; }


        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool b = false;
            FontWeight selected = FontWeights.Normal;
            FontWeight isTrueWeight = (IsTrueWeight != null) ? IsTrueWeight : FontWeights.Bold;
            FontWeight isFalseWeight = (IsFalseWeight != null) ? IsFalseWeight : FontWeights.Normal;

            if (value != null && Boolean.TryParse(value.ToString(), out b))
            {
                selected = (b) ? isTrueWeight : IsFalseWeight;
            }
            return selected;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
