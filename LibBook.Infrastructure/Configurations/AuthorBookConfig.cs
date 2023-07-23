using LibBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibBook.Infrastructure.Configurations;

public class AuthorBookConfig : IEntityTypeConfiguration<AuthorBook>
{
    public void Configure(EntityTypeBuilder<AuthorBook> builder)
    {
        builder.HasKey(cs => new { cs.AuthorId, cs.BookId });
        //builder.ToTable("AuthorBooks");

        builder.HasOne(cs => cs.Author)
            .WithMany(c => c.AuthorBooks)
            .HasForeignKey(cs => cs.AuthorId);

        builder.HasOne(cs => cs.Book)
            .WithMany(s => s.AuthorBooks)
            .HasForeignKey(cs => cs.BookId);
    }
}
