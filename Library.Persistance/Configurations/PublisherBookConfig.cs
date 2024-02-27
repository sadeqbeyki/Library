using Library.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistance.Configurations;

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
