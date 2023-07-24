using LibIdentity.Domain.PasswordAgg;
using LibIdentity.Domain.RoleAgg;
using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibIdentity.Infrastructure;

public class IdentityDbContext : IdentityDbContext<User, Role, int>
{
    public DbSet<BadPassword> BadPasswords { get; set; }
    public DbSet<User> Users { get; set; }
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }
}
