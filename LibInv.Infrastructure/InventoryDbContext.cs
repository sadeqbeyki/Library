using LibInventory.Domain.InventoryAgg;
using LibInventory.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LibInventory.Infrastructure
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Inventory> Inventory { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(InventoryConfig).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
