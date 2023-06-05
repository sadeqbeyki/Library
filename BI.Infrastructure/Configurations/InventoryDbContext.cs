using BI.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;

namespace BI.Infrastructure.Configurations
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Inventory> Inventories { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly=(typeof(InventoryConfig)).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
