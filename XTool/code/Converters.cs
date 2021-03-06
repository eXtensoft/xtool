using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace XTool
{

    [ValueConversion(typeof(bool), typeof(double))]
    public class BooleanToDoubleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                bool val = System.Convert.ToBoolean(value);
                return (val ? 1.0 : 0.0);
            }
            return (null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is double)
            {
                var val = (double)value;
                return (val == 0);
            }
            return (null);
        }
    }

    [ValueConversion(typeof(double), typeof(string))]
    public class ProgressBarValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is double)
            {
                double val = System.Convert.ToDouble(value);
                string strValue = val.ToString("N0");
                return (strValue);
            }
            return (value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value);
        }

    }

    public class ProgressBarPercentageConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double value = System.Convert.ToDouble(values[0]);
            double minValue = System.Convert.ToDouble(values[1]);
            double maxValue = System.Convert.ToDouble(values[2]);
            if (minValue == maxValue)
                return ("~%");
            double val = 100 * (value - minValue) / (maxValue - minValue);
            string strValue = val.ToString("N0") + "%";
            return (strValue);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class PositionConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double x = System.Convert.ToDouble(values[0]);
            double size = System.Convert.ToDouble(values[1]);
            return (x - size);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class Rect2Converter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double width = System.Convert.ToDouble(values[0]);
            double height = System.Convert.ToDouble(values[1]);
            return (new Rect(0, 0, width, height));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class RectConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double x = System.Convert.ToDouble(values[0]);
            double y = System.Convert.ToDouble(values[1]);
            double width = System.Convert.ToDouble(values[2]);
            double height = System.Convert.ToDouble(values[3]);
            return (new Rect(x, y, width, height));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
