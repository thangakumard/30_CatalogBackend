using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.Responses;
using System.Linq.Expressions;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProductRepository
    {
        public async Task<ServiceResponse> CreateAsync(Product entity)
        {


            try
            {
                Product product = await GetByAsync(p => p.Sku == entity.Sku);
                if (product != null)
                {
                    return new ServiceResponse() { Success= false, Message = $"{entity.Sku} is already in the database!" };
                }

                Product currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.ProductId > 0)
                {
                    return new ServiceResponse() { Success = true, Message = $"{entity.Sku} Product is saved successfully" };
                }
                else
                {
                    return new ServiceResponse() { Success = false, Message = $"{entity.Sku} Error occured while creating the product" };
                }
             }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return new ServiceResponse() { Success = false , Message = "Error in Product CreateAsync" };
            }
        }

        public async Task<ServiceResponse> DeleteAsync(Product entity)
        {
            try
            {
                Product product = await GetByAsync(p => p.ProductId == entity.ProductId);

                if (product == null)
                {
                    return new ServiceResponse() { Success=false, Message = $"{product.Sku} is not found" };
                }
                context.Products.Remove(entity);
                await context.SaveChangesAsync();
                return new ServiceResponse() { Success = true, Message = $"{product.Sku} is deleted successfully!" };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return new ServiceResponse() { Success = false, Message = "Error while deleting the product" };
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {

            try
            {
                List<Product> products = await context.Products.AsNoTracking().ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw new Exception("Error occured in GetAllAsync()");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                Product product = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return product;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw new Exception("Error occured in GetByAsync()");
            }
            
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                Product product = await context.Products.FindAsync(id);
                return product;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw new Exception("Error occured in GetAllAsync()");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(Product entity)
        {
            try
            {
                Product product = await GetByAsync(p => p.ProductId == entity.ProductId);

                if (product == null)
                {
                    return new ServiceResponse() { Success = false, Message = $"{product.Sku} is not found" };
                }
                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new ServiceResponse() { Success = true, Message = $"{product.Sku} is updated successfully!" };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return new ServiceResponse() { Success = false, Message = "Error while updating the product" };
            }
        }
    }
}
