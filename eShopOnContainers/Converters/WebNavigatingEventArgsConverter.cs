using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace eShopOnContainers.Converters;

public class WebNavigatingEventArgsConverter : BaseConverterOneWay<WebNavigatingEventArgs, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(WebNavigatingEventArgs value, CultureInfo culture)
    {
        return value.Url;
    }
}