using System;
using System.Globalization;
using Microsoft.Maui;

namespace eShopOnContainers.Converters
{
    public class DoesNotHaveCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int intValue)
            {
                return intValue <= 0;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
