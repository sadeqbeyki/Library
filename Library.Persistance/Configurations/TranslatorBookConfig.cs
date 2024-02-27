using Library.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistance.Configurations;

public class TranslatorBookConfig : IEntityTypeConfiguration<BookTranslator>
{
    public void Configure(EntityTypeBuilder<BookTranslator> builder)
    {
        builder.ToTable("BookTranslators");
        builder.HasKey(cs => new { cs.TranslatorId, cs.TranslatorBookId });

        builder.HasOne(cs => cs.Translator)
            .WithMany(c => c.TranslatorBooks)
            .HasForeignKey(cs => cs.TranslatorId);

        builder.HasOne(cs => cs.Book)
            .WithMany(s => s.BookTranslators)
            .HasForeignKey(cs => cs.TranslatorBookId);
    }
}
