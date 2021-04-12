using System;
using System.Windows.Data;
using System.Windows;

namespace XTool
{
    public class IntToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility option = Visibility.Visible;
            int i;
            if (Int32.TryParse(value.ToString(), out i))
            {
                if (i == 0)
                {
                    option = Visibility.Collapsed;
                }
            }

            return option;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
