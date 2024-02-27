using Library.Domain.Entities.LendAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistance.Configurations;

public class LendConfig : IEntityTypeConfiguration<Lend>
{
    public void Configure(EntityTypeBuilder<Lend> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Loans");

        builder.Property(b => b.ReturnDate)
            .IsRequired(false);

        builder.Property(b => b.ReturnEmployeeID)
            .IsRequired(false);

        builder.Property(b => b.Description)
            .IsRequired(false);
    }
}