using LI.ApplicationContracts.Password;
using LI.Domain.RoleAgg;
using LI.Domain.UserAgg;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LI.Infrastructure;

public class LiIdentityDbContext : IdentityDbContext<User, Role, string>
{
    public DbSet<BadPassword> BadPasswords { get; set; }
    public DbSet<User> Users { get; set; }
    public LiIdentityDbContext(DbContextOptions<LiIdentityDbContext> options) : base(options)
    {
    }
}
