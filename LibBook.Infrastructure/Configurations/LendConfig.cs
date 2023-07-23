using LibBook.Domain.LendAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibBook.Infrastructure.Configurations;

public class LendConfig : IEntityTypeConfiguration<Lend>
{
    public void Configure(EntityTypeBuilder<Lend> builder)
    {
        builder.ToTable("Lends");
        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Items, modelBuilder =>
        {
            modelBuilder.HasKey(x => x.Id);
            modelBuilder.ToTable("LendItems");
            modelBuilder.WithOwner(x => x.Lend).HasForeignKey(x => x.LendId);
        });
    }
}