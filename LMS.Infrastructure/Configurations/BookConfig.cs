using LMS.Domain;
using LMS.Domain.BookAgg;
using LMS.Domain.BookCategoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
        builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Code).IsRequired().HasMaxLength(10);
        builder.Property(b => b.Description).HasMaxLength(500);

        builder.HasOne(bc => bc.Category).WithMany(b => b.Books).HasForeignKey(fk => fk.CategoryId);


        //builder.HasMany(b => b.Authors)
        //    .WithMany(a => a.Books)
        //    .UsingEntity<AuthorBook>(
        //        j => j.HasOne(ab => ab.Author)
        //            .WithMany(a => a.AuthorBooks)
        //            .HasForeignKey(ab => ab.AuthorId),
        //        j => j.HasOne(ab => ab.Book)
        //            .WithMany(b => b.AuthorBooks)
        //            .HasForeignKey(ab => ab.BookId),
        //        j =>
        //        {
        //            j.ToTable("AuthorBooks");

        //            // Define composite key
        //            j.HasKey(ab => new { ab.AuthorId, ab.BookId });
        //        });

        //builder.HasMany(b => b.Publishers)
        //    .WithMany(p => p.Books)
        //    .UsingEntity<PublisherBook>(
        //        j => j.HasOne(pb => pb.Publisher)
        //            .WithMany(p => p.PublisherBooks)
        //            .HasForeignKey(pb => pb.PublisherId),
        //        j => j.HasOne(pb => pb.Book)
        //            .WithMany(b => b.PublisherBooks)
        //            .HasForeignKey(pb => pb.BookId),
        //        j =>
        //        {
        //            j.ToTable("PublisherBooks");

        //            // Define composite key
        //            j.HasKey(pb => new { pb.PublisherId, pb.BookId });
        //        });

        //builder.HasMany(b => b.Translators)
        //    .WithMany(t => t.Books)
        //    .UsingEntity<TranslatorBook>(
        //        j => j.HasOne(tb => tb.Translator)
        //            .WithMany(t => t.TranslatorBooks)
        //            .HasForeignKey(tb => tb.TranslatorId),
        //        j => j.HasOne(tb => tb.Book)
        //            .WithMany(b => b.TranslatorBooks)
        //            .HasForeignKey(tb => tb.BookId),
        //        j =>
        //        {
        //            j.ToTable("TranslatorBooks");

        //            // Define composite key
        //            j.HasKey(tb => new { tb.TranslatorId, tb.BookId });
        //        });
    }
}

//public class AuthorConfig : IEntityTypeConfiguration<Author>
//{
//    public void Configure(EntityTypeBuilder<Author> builder)
//    {
//        builder.HasKey(a => a.Id);

//        builder.HasMany(b => b.Books)
//    .WithMany(a => a.Authors)
//    .UsingEntity<AuthorBook>(
//        j => j.HasOne(ab => ab.Book)
//            .WithMany(a => a.AuthorBooks)
//            .HasForeignKey(ab => ab.BookId),
//        j => j.HasOne(ab => ab.Author)
//            .WithMany(b => b.AuthorBooks)
//            .HasForeignKey(ab => ab.AuthorId),
//        j =>
//        {
//            j.ToTable("AuthorBooks");

//            // Define composite key
//            j.HasKey(ab => new { ab.AuthorId, ab.BookId });
//        });
//    }
//}

//public class TranslatorConfig : IEntityTypeConfiguration<Translator>
//{
//    public void Configure(EntityTypeBuilder<Translator> builder)
//    {
//        builder.HasKey(a => a.Id);

//        builder.HasMany(b => b.Books)
//    .WithMany(a => a.Translators)
//    .UsingEntity<TranslatorBook>(
//        j => j.HasOne(ab => ab.Book)
//            .WithMany(a => a.TranslatorBooks)
//            .HasForeignKey(ab => ab.BookId),
//        j => j.HasOne(ab => ab.Translator)
//            .WithMany(b => b.TranslatorBooks)
//            .HasForeignKey(ab => ab.TranslatorId),
//        j =>
//        {
//            j.ToTable("TranslatorBooks");

//            // Define composite key
//            j.HasKey(ab => new { ab.TranslatorId, ab.BookId });
//        });
//    }
//}

//public class PublisherConfig : IEntityTypeConfiguration<Publisher>
//{
//    public void Configure(EntityTypeBuilder<Publisher> builder)
//    {
//        builder.HasKey(a => a.Id);

//        builder.HasMany(b => b.Books)
//    .WithMany(a => a.Publishers)
//    .UsingEntity<PublisherBook>(
//        j => j.HasOne(ab => ab.Book)
//            .WithMany(a => a.PublisherBooks)
//            .HasForeignKey(ab => ab.BookId),
//        j => j.HasOne(ab => ab.Publisher)
//            .WithMany(b => b.PublisherBooks)
//            .HasForeignKey(ab => ab.PublisherId),
//        j =>
//        {
//            j.ToTable("PublisherBooks");

//            // Define composite key
//            j.HasKey(ab => new { ab.PublisherId, ab.BookId });
//        });
//    }
//}

public class BookCategoryConfig : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        builder.HasKey(a => a.Id);
        builder.ToTable("BookCategories");

        builder.HasMany(b => b.Books).WithOne(c => c.Category).HasForeignKey(fk => fk.CategoryId);

    }
}

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


