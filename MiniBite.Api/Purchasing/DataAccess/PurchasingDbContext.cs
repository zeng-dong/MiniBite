using Microsoft.EntityFrameworkCore;
using MiniBite.Api.Purchasing.Entities;

namespace MiniBite.Api.Purchasing.DataAccess
{
    public class PurchasingDbContext : DbContext
    {
        public PurchasingDbContext(DbContextOptions<PurchasingDbContext> options) : base(options)
        {
        }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<Item> Items { get; set; }

    }
}
