using LibBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibBook.Infrastructure.Configurations;

public class AuthorBookConfig : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.ToTable("BookAuthors");
        builder.HasKey(cs => new { cs.AuthorId, cs.AuthorBookId });

        builder.HasOne(cs => cs.Author)
            .WithMany(c => c.AuthorBooks)
            .HasForeignKey(cs => cs.AuthorId);

        builder.HasOne(cs => cs.Book)
            .WithMany(s => s.BookAuthors)
            .HasForeignKey(cs => cs.AuthorBookId);
    }
}
