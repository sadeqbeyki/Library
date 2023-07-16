using LMS.Domain.BookAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
        builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Code).IsRequired().HasMaxLength(10);
        builder.Property(b => b.Description).HasMaxLength(500);

        builder.HasOne(bc => bc.Category).WithMany(b => b.Books).HasForeignKey(fk => fk.CategoryId);
    }
}