using LibBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibBook.Infrastructure.Configurations;

public class TranslatorBookConfig : IEntityTypeConfiguration<TranslatorBook>
{
    public void Configure(EntityTypeBuilder<TranslatorBook> builder)
    {
        builder.HasKey(cs => new { cs.TranslatorId, cs.BookId });
        //builder.ToTable("TranslatorBooks");

        builder.HasOne(cs => cs.Translator)
            .WithMany(c => c.TranslatorBooks)
            .HasForeignKey(cs => cs.TranslatorId);

        builder.HasOne(cs => cs.Book)
            .WithMany(s => s.TranslatorBooks)
            .HasForeignKey(cs => cs.BookId);
    }
}
