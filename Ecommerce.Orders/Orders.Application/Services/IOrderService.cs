using Orders.Application.DTOs;

namespace Orders.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersByClientId(int clientId);
        Task<OrderDetailDto> getOrderDetails(int orderId);

    }
}
