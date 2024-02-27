using Library.Domain.Entities.AuthorAgg;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.BookCategoryAgg;
using Library.Domain.Entities.Common;
using Library.Domain.Entities.LendAgg;
using Library.Domain.Entities.PublisherAgg;
using Library.Domain.Entities.TranslatorAgg;
using Library.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance;

public class BookDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Translator> Translators { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<BookAuthor> AuthorBooks { get; set; }
    public DbSet<BookTranslator> TranslatorBooks { get; set; }
    public DbSet<BookPublisher> PublisherBooks { get; set; }
    public DbSet<Lend> Loans { get; set; }

    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(BookConfig).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}