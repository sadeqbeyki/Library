using LibBook.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibBook.Infrastructure.Configurations;

public class TranslatorBookConfig : IEntityTypeConfiguration<BookTranslator>
{
    public void Configure(EntityTypeBuilder<BookTranslator> builder)
    {
        builder.HasKey(cs => new { cs.TranslatorId, cs.TranslatorBookId });
        //builder.ToTable("TranslatorBooks");

        builder.HasOne(cs => cs.Translator)
            .WithMany(c => c.TranslatorBooks)
            .HasForeignKey(cs => cs.TranslatorId);

        builder.HasOne(cs => cs.Book)
            .WithMany(s => s.BookTranslators)
            .HasForeignKey(cs => cs.TranslatorBookId);
    }
}
