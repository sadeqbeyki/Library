using Identity.Domain.Entities.AuthAgg;
using Identity.Domain.Entities.Role;
using Identity.Domain.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistance;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public DbSet<BadPassword> BadPasswords { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
    }
}
