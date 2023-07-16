using LMS.Domain.BookCategoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

public class BookCategoryConfig : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.HasKey(a => a.Id);
        builder.ToTable("BookCategories");

        builder.HasMany(b => b.Books).WithOne(c => c.Category).HasForeignKey(fk => fk.CategoryId);

    }
}
