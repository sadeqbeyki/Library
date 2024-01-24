using LibIdentity.ApplicationServices;
using LibIdentity.Domain.RoleAgg;
using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.Auth;
using LibIdentity.DomainContracts.RoleContracts;
using LibIdentity.DomainContracts.UserContracts;
using LibIdentity.Infrastructure;
using LibIdentity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LibIdentityConfiguration;

public static class ServiceExtentions
{
    public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<UserIdentity, RoleIdentity>(i =>
        {
            i.SignIn.RequireConfirmedAccount = false;
            i.SignIn.RequireConfirmedEmail = true;
            i.SignIn.RequireConfirmedPhoneNumber = false;

            i.User.RequireUniqueEmail = true;

            //c.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmPOIUYTREWQLKJHGFDSAMNBVCXZ";
            i.Password.RequireDigit = false;
            i.Password.RequireNonAlphanumeric = false;
            i.Password.RequireLowercase = false;
            i.Password.RequireUppercase = false;
            i.Password.RequiredUniqueChars = 1;
            i.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<IdentityDbContext>();

        services.AddIdentityCore<UserIdentity>(options => options.SignIn.RequireConfirmedAccount = true)
                        .AddEntityFrameworkStores<IdentityDbContext>()
                        .AddTokenProvider<DataProtectorTokenProvider<UserIdentity>>(TokenOptions.DefaultProvider);

        services.AddScoped<IPasswordValidator<UserIdentity>, LIPasswordValidator>();
        services.AddScoped<IUserValidator<UserIdentity>, LIUserValidator>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();


        services.AddDbContext<IdentityDbContext>(c =>
            c.UseSqlServer(configuration.GetConnectionString("AAA")));

        //services.AddScoped<IIdentityService, IdentityService>();
    }

    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        //Jwt configuration starts here
        services.AddScoped<IJwtService, JwtService>();
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience =configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });
        services.AddAuthorization();
    }

    public static void CreateIdentityDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        dataContext.Database.EnsureCreated();
        //dataContext.Database.Migrate();
    }
}
