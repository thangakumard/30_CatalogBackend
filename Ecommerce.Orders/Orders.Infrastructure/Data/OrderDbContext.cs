using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using System.Security.Cryptography.X509Certificates;

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
