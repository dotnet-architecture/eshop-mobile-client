using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace eShopOnContainers.Converters
{
    public class WebNavigatedEventArgsConverter : ICommunityToolkitValueConverter
    {
        public Type FromType => typeof(WebNavigatedEventArgs);

        public Type ToType => typeof(string);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as WebNavigatedEventArgs;
            if (eventArgs == null)
                throw new ArgumentException("Expected WebNavigatedEventArgs as value", "value");

            return eventArgs.Url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}