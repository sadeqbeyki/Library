using LMS.Domain;
using LMS.Domain.AuthorAgg;
using LMS.Domain.BookAgg;
using LMS.Domain.BookCategoryAgg;
using LMS.Domain.BorrowAgg;
using LMS.Domain.LendAgg;
using LMS.Domain.PublisherAgg;
using LMS.Domain.TranslatorAgg;
using LMS.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure;

public class BookDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors  { get; set; }
    public DbSet<Translator> Translators{ get; set; }
    public DbSet<Publisher> Publishers{ get; set; }
    public DbSet<BookCategory> BookCategories{ get; set; }
    public DbSet<AuthorBook> AuthorBooks{ get; set; }
    public DbSet<TranslatorBook> TranslatorBooks{ get; set; }
    public DbSet<PublisherBook> PublisherBooks{ get; set; }

    public DbSet<Lend> Lends { get; set; }
    public DbSet<Borrow> Borrows { get; set; }

    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(BookConfig).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}