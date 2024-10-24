using Orders.Application.DTOs;
using Orders.Application.DTOs.Conversion;
using Orders.Application.Interfaces;
using Polly;
using Polly.Registry;
using System.Net.Http.Json;

namespace Orders.Application.Services
{
    public class OrderService : IOrderService
    {
        HttpClient _httpClient;
        ResiliencePipelineProvider<string> _resiliencePipeline;
        IOrderRepository _orderIntrface;
        public OrderService(IOrderRepository orderIntrface, 
            HttpClient httpClient,
            ResiliencePipelineProvider<string> resiliencePipeline
            )
        {
            _httpClient = httpClient;
            _orderIntrface = orderIntrface;
            _resiliencePipeline = resiliencePipeline;
        }

        
        /// <summary>
        /// Get Products using productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>        
        public async Task<ProductDto> GetProduct(int productId)
        {
            //Call product API
            //Redirect this call to the API Gateway
            var getProduct = await _httpClient.GetAsync($"/api/Products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
            {
                return null;
            }

            ProductDto product = await getProduct.Content.ReadFromJsonAsync<ProductDto>();
            return product;
        }

        public async Task<AppUserDto> GetUser(int userId)
        {
            //Call product API
            //Redirect this call to the API Gateway
            var getUser = await _httpClient.GetAsync($"/api/Products/{userId}");
            if (!getUser.IsSuccessStatusCode)
            {
                return null!;
            }

            AppUserDto appUserDto = await getUser.Content.ReadFromJsonAsync<AppUserDto>();
            return appUserDto;
        }
        public async Task<OrderDetailDto> getOrderDetailsAsync(int orderId)
        {
            var order = await _orderIntrface.GetByIdAsync(orderId);
            if(order == null || order!.OrderId <= 0)
            {
                return null!;
            }

            //Get retry
            var retryPipeline = _resiliencePipeline.GetPipeline("orders-retry-pipeline");

            //Prepare product
            ProductDto productDto = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            //prepare Client
            AppUserDto appUserDto = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDetailDto(
                order.OrderId,
                productDto.ProductId, 
                appUserDto.UserId,
                appUserDto.UserName,
                appUserDto.Email,
                appUserDto.Address,
                appUserDto.PhoneNumber,
                productDto.Sku,
                order.PurchaseQuantity,
                productDto.Price,
                productDto.Quantity * order.PurchaseQuantity,
                order.OrderDate
                );
        }

        /// <summary>
        ///    
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderDto>> GetOrdersByClientIdAsync(int clientId)
        {
            var orders = await _orderIntrface.GetOrderAsync(o => o.ClientId == clientId);
            if (!orders.Any())
            {
                return null!;
            }

            //Conver entiry to DTO
            var (_, _orders)  =OrderConversion.FromEntity(null, orders);
            return _orders; 
        }
    }
}
