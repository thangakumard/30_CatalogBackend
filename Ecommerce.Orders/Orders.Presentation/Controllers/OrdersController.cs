using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTOs;
using Orders.Application.DTOs.Conversion;
using Orders.Application.Interfaces;
using Orders.Application.Services;
using SharedLibrary.Responses;

namespace Orders.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        IOrderRepository _orderRepository;
        IOrderService _orderService;
        public OrdersController(IOrderRepository orderRepository, IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrder()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (!orders.Any())
            {
                return NotFound();
            }
            var (_, list) = OrderConversion.FromEntity(null, orders);
            return Ok(list);
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound();

            var (_order, _) = OrderConversion.FromEntity(order, null);
            return Ok(_order);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDto>> GetClientOrders(int clientId)
        {
            var orders = await _orderService.GetOrdersByClientIdAsync(clientId);
            return !orders.Any() ? NotFound() : Ok(orders);
        }

        [HttpGet("detail/{orderId:int}")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetail(int orderId)
        {
            if (orderId <= 0) return NotFound();
            var orderDetail = await _orderService.getOrderDetailsAsync(orderId);
            return orderDetail.OrderId > 0 ? Ok(orderDetail) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var getEntity = OrderConversion.ToEntity(orderDto);
            var response = await _orderRepository.CreateAsync(getEntity);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> UpdateOrder(OrderDto orderDto)
        {
            var order = OrderConversion.ToEntity(orderDto);
            var response = await _orderRepository.UpdateAsync(order);
            return response.Success ? Ok(response) : BadRequest(response);

        }

        [HttpDelete("{orderId:int}")]
        public async Task<ActionResult<ServiceResponse>> DeleteOrder(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return NotFound();
            var response = await _orderRepository.DeleteAsync(order);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
