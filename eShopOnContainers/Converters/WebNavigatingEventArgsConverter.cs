using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace eShopOnContainers.Converters;

public class WebNavigatingEventArgsConverter : ICommunityToolkitValueConverter
{
    public Type FromType => typeof(WebNavigatingEventArgs);

    public Type ToType => typeof(string);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is not WebNavigatingEventArgs eventArgs
            ? throw new ArgumentException("Expected WebNavigatingEventArgs as value", nameof(value))
            : (object)eventArgs.Url;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}