using LibIdentity.Domain.PasswordAgg;
using LibIdentity.Domain.RoleAgg;
using LibIdentity.Domain.UserAgg;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibIdentity.Infrastructure;

public class IdentityDbContext : IdentityDbContext<UserIdentity, RoleIdentity, int>
{
    public DbSet<BadPassword> BadPasswords { get; set; }
    public DbSet<UserIdentity> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }
}
