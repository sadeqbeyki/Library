using LibBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibBook.Infrastructure.Configurations;

public class PublisherBookConfig : IEntityTypeConfiguration<BookPublisher>
{
    public void Configure(EntityTypeBuilder<BookPublisher> builder)
    {
        builder.ToTable("BookPublishers");
        builder.HasKey(cs => new { cs.PublisherId, cs.PublisherBookId });

        builder.HasOne(cs => cs.Publisher)
            .WithMany(c => c.PublisherBooks)
            .HasForeignKey(cs => cs.PublisherId);

        builder.HasOne(cs => cs.Book)
            .WithMany(s => s.BookPublishers)
            .HasForeignKey(cs => cs.PublisherBookId);
    }
}
