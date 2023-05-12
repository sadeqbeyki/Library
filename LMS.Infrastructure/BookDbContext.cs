using BMS.Domain.BookAgg;
using LMS.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LMS.Infrastructure
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public BookDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.;" +
                "Database=LibraryDB;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;" +
                "User ID=sa;" +
                "Password=7410");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(BookConfig).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}