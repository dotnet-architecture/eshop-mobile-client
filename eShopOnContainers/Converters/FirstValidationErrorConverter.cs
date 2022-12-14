using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace eShopOnContainers.Converters;

public class FirstValidationErrorConverter : BaseConverterOneWay<ICollection<string>, string>
{
    public override string DefaultConvertReturnValue { get; set; } = string.Empty;

    public override string ConvertFrom(ICollection<string> value, CultureInfo culture)
    {
        return value?.FirstOrDefault() ?? DefaultConvertReturnValue;
    }
}
