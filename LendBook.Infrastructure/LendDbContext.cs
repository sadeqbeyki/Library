using LendBook.Domain.EmployeeAgg;
using LendBook.Domain.LendAgg;
using LendBook.Domain.MemberAgg;
using LendBook.Domain.RentAgg;
using LendBook.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LendBook.Infrastructure;

public class LendDbContext : DbContext
{

    //public DbSet<Borrow> Reservations{ get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<Lend> Lends { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public LendDbContext(DbContextOptions<LendDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(LendConfig).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}