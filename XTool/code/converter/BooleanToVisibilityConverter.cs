using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace XTool
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private Visibility _FalseValue = Visibility.Collapsed;
        public Visibility FalseValue 
        {
            get { return _FalseValue; }
            set { _FalseValue = value; }
        }

        public bool IsReverse { get; set; }
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool b = false;
            Visibility selected = Visibility.Visible;
            if (value != null && Boolean.TryParse(value.ToString(), out b))
            {
                if (!IsReverse)
                {
                    selected = (b) ? Visibility.Visible : FalseValue;
                }
                else
                {
                    selected = (!b) ? Visibility.Visible : FalseValue;
                }
                
            }
            return selected;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
