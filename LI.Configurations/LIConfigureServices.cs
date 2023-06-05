using LI.ApplicationContracts.Auth;
using LI.ApplicationServices;
using LI.Domain.RoleAgg;
using LI.Domain.UserAgg;
using LI.Infrastructure;
using LI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LI.Configurations;

public static class LIConfigureServices
{
    public static void Configure(IServiceCollection services, string? connectionString)
    {

        services.AddDbContext<LiIdentityDbContext>(c => c.UseSqlServer(connectionString));

        services.AddScoped<IPasswordValidator<User>, LIPasswordValidator>();
        services.AddScoped<IUserValidator<User>, LIUserValidator>();
        services.AddTransient<IAuthHelper, AuthHelper>();
    }
}