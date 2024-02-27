using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Domain.Entities.InventoryAgg;

namespace Warehouse.Persistance.Configurations;

public class InventoryConfig : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventory");
        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Operations, modelBuilder =>
        {
            modelBuilder.HasKey(x => x.Id);
            modelBuilder.ToTable("InventoryOperations");
            modelBuilder.Property(x => x.Description).HasMaxLength(1000);
            modelBuilder.WithOwner(x => x.Inventory).HasForeignKey(x => x.InventoryId);
        });
    }
}
