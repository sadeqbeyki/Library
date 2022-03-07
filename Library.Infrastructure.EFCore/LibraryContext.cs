using Library.Domain.BookAgg;
using Library.Domain.BookCategoryAgg;
using Library.Infrastructure.EFCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.EFCore
{
    public class LibraryContext : DbContext
    {
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Book> Books { get; set; }
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(BookCategoryMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
