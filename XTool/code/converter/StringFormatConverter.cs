using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace XTool
{
    public class StringFormatConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            if (parameter != null && !String.IsNullOrEmpty(parameter.ToString()))
            {
                string key = parameter.ToString();
                Dictionary<string, string> d = Application.Current.Properties[AppConstants.FormatStrings] as Dictionary<string, string>;
                if (d != null && d.ContainsKey(key))
                {
                    string text = d[key];
                    return String.Format(text, o.ToString());
                }
            }
            return o.ToString();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
