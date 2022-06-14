using System;
using System.Globalization;
using eShopOnContainers.Models.Orders;

namespace eShopOnContainers.Converters
{
	public class OrderStatusToStringConverter : IValueConverter
	{

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is OrderStatus orderStatus)
            {
                switch (orderStatus)
                {
                    case OrderStatus.AwaitingValidation:
                        return "AWAITING VALIDATION";
                    case OrderStatus.Cancelled:
                        return "CANCELLED";
                    case OrderStatus.Paid:
                        return "PAID";
                    case OrderStatus.Shipped:
                        return "SHIPPED";
                    case OrderStatus.StockConfirmed:
                        return "STOCK CONFIRMED";
                    case OrderStatus.Submitted:
                        return "SUBMITTED";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

