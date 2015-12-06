using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EnterpriseApps.UWP.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(Visibility))
            {
                if (parameter is string && ((string)parameter).Equals("r", StringComparison.CurrentCultureIgnoreCase))
                    return !(value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
                else
                    return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }

            if (targetType == typeof(double) || targetType == typeof(int))
            {
                if (parameter is string && ((string)parameter).Equals("r", StringComparison.CurrentCultureIgnoreCase))
                    return !(value is bool && (bool)value) ? 1 : 0;
                else
                    return (value is bool && (bool)value) ? 1 : 0;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
