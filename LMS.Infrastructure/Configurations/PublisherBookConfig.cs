using LMS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

public class PublisherBookConfig : IEntityTypeConfiguration<PublisherBook>
{
    public void Configure(EntityTypeBuilder<PublisherBook> builder)
    {
        builder.HasKey(cs => new { cs.PublisherId, cs.BookId });
        //builder.ToTable("PublisherBooks");

        builder.HasOne(cs => cs.Publisher)
            .WithMany(c => c.PublisherBooks)
            .HasForeignKey(cs => cs.PublisherId);

        builder.HasOne(cs => cs.Book)
            .WithMany(s => s.PublisherBooks)
            .HasForeignKey(cs => cs.BookId);
    }
}
