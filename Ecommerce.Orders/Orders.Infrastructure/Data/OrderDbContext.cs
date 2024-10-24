using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;

namespace Orders.Infrastructure.Data
{
    public class OrderDbContext : DbContext
    {
        DbContextOptions<OrderDbContext> _options;
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            _options = options;
        }
        public DbSet<Order> Orders { get; set; }

    }
}
