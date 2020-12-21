using Microsoft.EntityFrameworkCore;
using MiniBite.Api.Inventory.Entities;

namespace MiniBite.Api.Inventory.DataAccess
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Proudcts { get; set; }
    }
}
