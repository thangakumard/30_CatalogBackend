using Orders.Application.DTOs;

namespace Orders.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersByClientIdAsync(int clientId);
        Task<OrderDetailDto> getOrderDetailsAsync(int orderId);

    }
}
