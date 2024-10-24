using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;
using Orders.Infrastructure.Data;
using SharedLibrary;
using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace Orders.Infrastructure.Repositories
{
    public class OrderRepository : IOrder
    {
        OrderDbContext _orderDbContext;
        public OrderRepository(OrderDbContext dbContext) {
            _orderDbContext = dbContext;
        }
        public async Task<ServiceResponse> CreateAsync(Order entity)
        {
            try
            {
                var order = _orderDbContext.Orders.Add(entity).Entity;
                await _orderDbContext.SaveChangesAsync();
                return order.OrderId > 0 ? new ServiceResponse() { Success=true, Message="Order has been received successfully!"} : 
                    new ServiceResponse() { Success = false, Message="Error Occured!"};
            }
            catch (Exception ex) { 
                //Log the original exception
                Logger.LogError(ex);

                //Display the error message to the client
                return new ServiceResponse() { Success = false, Message = "Error Occurred!" };

            }
        }

        public async Task<ServiceResponse> DeleteAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.OrderId);
                if(order == null) return new ServiceResponse() { Success = false, Message="Order is not found!"};
                _orderDbContext.Orders.Remove(entity);
                await _orderDbContext.SaveChangesAsync();
                return new ServiceResponse() { Success = true, Message = "Error Occured!" };
            }
            catch (Exception ex)
            {
                //Log the original exception
                Logger.LogError(ex);

                //Display the error message to the client
                return new ServiceResponse() { Success = false, Message = "Error Occurred!" };

            }
        
    }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await _orderDbContext.Orders.AsNoTracking().ToListAsync();
                return orders!;
            }
            catch (Exception ex)
            {
                //Log the original exception
                Logger.LogError(ex);

                //Display the error message to the client
                throw new Exception("Error Occured!");

            }
        
    }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var Order = await _orderDbContext.Orders.Where(predicate).FirstOrDefaultAsync();
                return Order!;
            }
            catch (Exception ex)
            {
                //Log the original exception
                Logger.LogError(ex);

                //Display the error message to the client
                throw new Exception(ex.Message);
            }
        }
    

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderDbContext.Orders!.FindAsync(id);
                return order!;
            }
            catch (Exception ex)
            {
                //Log the original exception
                Logger.LogError(ex);

                //Display the error message to the client
                throw new Exception("Error occured!");
            }
        }
    

        public async Task<IEnumerable<Order>> GetOrderAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await _orderDbContext.Orders.Where(predicate).ToListAsync();
                return orders!;
            }
            catch (Exception ex)
            {
                //Log the original exception
                Logger.LogError(ex);

                //Display the error message to the client
                throw new Exception(ex.Message);
            }
        }
    

        public async Task<ServiceResponse> UpdateAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.OrderId);
                if (order == null) return new ServiceResponse() { Success = false, Message = "Order is not avaialble" };
                _orderDbContext.Entry(entity).State = EntityState.Detached;
                _orderDbContext.Orders.Update(entity);
                await _orderDbContext.SaveChangesAsync();
                return new ServiceResponse() { Success = true, Message = "Order has been updated successfully!" };
            }
            catch (Exception ex)
            {
                //Log the original exception
                Logger.LogError(ex);

                //Display the error message to the client
                return new ServiceResponse() { Success = false, Message = "Error Occurred!" };

            }
        }
    
    }
}
