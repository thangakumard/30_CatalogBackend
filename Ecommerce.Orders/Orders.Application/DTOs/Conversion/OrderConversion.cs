using Orders.Domain.Entities;

namespace Orders.Application.DTOs.Conversion
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDto order) => new Order()
        {
            OrderId = order.OrderId,
            ProductId = order.ProductId,
            ClientId = order.ClientId,
            OrderDate = order.OrderDate,
            PurchaseQuantity = order.PurchaseQuantity
        };

        public static (OrderDto?, IEnumerable<OrderDto>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            if(order != null && orders is null)
            {
                var SingleOrder = new OrderDto(
                    order.OrderId, 
                    order.ProductId,
                    order.ClientId,
                    order.PurchaseQuantity,
                    order.OrderDate);
                return (SingleOrder, null);
            }
            if(order == null && orders is not  null)
            {
                var _orders = orders.Select(o => new OrderDto(
                    o.OrderId,
                    o.ProductId,
                    o.ClientId,
                    o.PurchaseQuantity,
                    o.OrderDate
                    ));
                return (null, _orders);
            }
            return (null, null);
        }
    }
}
