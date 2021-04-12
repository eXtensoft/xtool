using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace XTool
{
    public class ImageMapConverter : IValueConverter
    {

        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = "images/default.png";
            string prefix = (parameter == null) ? String.Empty : parameter.ToString();
            if (value != null && !String.IsNullOrEmpty(value.ToString()))
            {
                List<Tuple<string, string>> list = Application.Current.Properties[AppConstants.ImageMaps] as List<Tuple<string, string>>;
                if (list != null)
                {

                    Tuple<string, string> found = list.FirstOrDefault(x => x.Item1.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));
                    if (found != null)
                    {
                        path = (!String.IsNullOrEmpty(prefix)) ? prefix + found.Item2 : found.Item2;
                    }
                    else
                    {
                        path = (!String.IsNullOrEmpty(prefix)) ? prefix + path : path;
                    }

                }
            }
            return path;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
