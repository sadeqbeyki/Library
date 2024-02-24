using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities.InventoryAgg;
using Warehouse.Persistance.Configurations;

namespace Warehouse.Persistance
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
