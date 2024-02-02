using Identity.Domain.Entities.AuthAgg;
using Identity.Domain.Entities.Role;
using Identity.Domain.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistance;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<BadPassword> BadPasswords { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
    }

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    var assembly = typeof(TokenMapping).Assembly;
    //    builder.ApplyConfigurationsFromAssembly(assembly);
    //    base.OnModelCreating(builder);
    //}
}
