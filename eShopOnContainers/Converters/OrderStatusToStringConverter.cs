using System.Globalization;
using eShopOnContainers.Models.Orders;

namespace eShopOnContainers.Converters;

public class OrderStatusToStringConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            OrderStatus.AwaitingValidation => "AWAITING VALIDATION",
            OrderStatus.Cancelled => "CANCELLED",
            OrderStatus.Paid => "PAID",
            OrderStatus.Shipped => "SHIPPED",
            OrderStatus.StockConfirmed => "STOCK CONFIRMED",
            OrderStatus.Submitted => "SUBMITTED",
            _ => string.Empty
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

