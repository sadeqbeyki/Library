using BI.Domain.InventoryAgg;
using BI.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BI.Infrastructure
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
