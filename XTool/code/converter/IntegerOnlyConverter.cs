using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace XTool
{
    public class IntegerOnlyConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int i = 0;
            if (value != null && Int32.TryParse(value.ToString(), out i))
            {

            }
            return i;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int i = 0;
            if (value != null && Int32.TryParse(value.ToString(), out i))
            {

            }
            return i;
        }

        #endregion
    }
}
