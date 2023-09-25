using LibBook.Domain;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.Domain.BookCategoryAgg;
using LibBook.Domain.BorrowAgg;
using LibBook.Domain.PublisherAgg;
using LibBook.Domain.TranslatorAgg;
using LibBook.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure;

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
    public DbSet<Borrow> Borrows { get; set; }

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