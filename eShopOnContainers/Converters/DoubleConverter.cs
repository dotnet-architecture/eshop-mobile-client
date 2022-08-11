using System.Globalization;

namespace eShopOnContainers.Converters;

public class DoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is double
            ? value.ToString()
            : value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        double.TryParse(value as string, out double doub)
            ? doub 
            : value;
}
