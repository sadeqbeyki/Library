using Library.Domain.Entities.AuthorAgg;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.PublisherAgg;
using Library.Domain.Entities.TranslatorAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistance.Configurations;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Books");

        builder.Property(b => b.Title).IsRequired().HasMaxLength(250);
        builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Code).IsRequired().HasMaxLength(10);
        builder.Property(b => b.Description).HasMaxLength(750);

        builder.HasOne(bc => bc.Category)
            .WithMany(b => b.Books)
            .HasForeignKey(fk => fk.CategoryId);


        // 
        builder.HasMany(e => e.BookAuthors)
            .WithOne(e => e.Book)
            .HasForeignKey(e => e.AuthorBookId);

        builder.HasMany(e => e.BookPublishers)
            .WithOne(e => e.Book)
            .HasForeignKey(e => e.PublisherBookId);

        builder.HasMany(e => e.BookTranslators)
            .WithOne(e => e.Book)
            .HasForeignKey(e => e.TranslatorBookId);
    }
}

public class AuthorConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Authors");

        builder.Property(b => b.Name).IsRequired().HasMaxLength(250);
        builder.Property(b => b.Description).HasMaxLength(750);

        // 
        builder.HasMany(e => e.AuthorBooks)
            .WithOne(e => e.Author)
            .HasForeignKey(e => e.AuthorId);
    }
}

public class PublisherConfig : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Publishers");

        builder.Property(b => b.Name).IsRequired().HasMaxLength(250);
        builder.Property(b => b.Description).HasMaxLength(750);

        // 
        builder.HasMany(e => e.PublisherBooks)
            .WithOne(e => e.Publisher)
            .HasForeignKey(e => e.PublisherId);
    }
}

public class TranslatorConfig : IEntityTypeConfiguration<Translator>
{
    public void Configure(EntityTypeBuilder<Translator> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Translators");

        builder.Property(b => b.Name).IsRequired().HasMaxLength(250);
        builder.Property(b => b.Description).HasMaxLength(750);

        // 
        builder.HasMany(e => e.TranslatorBooks)
            .WithOne(e => e.Translator)
            .HasForeignKey(e => e.TranslatorId);
    }
}
