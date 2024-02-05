using Identity.Application.Interfaces;
using Identity.Domain.Entities.Role;
using Identity.Domain.Entities.User;
using Identity.Persistance;
using Identity.Persistance.Repositories;
using Identity.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity.Services;

public static class ServiceExtentions
{
    public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(i =>
        {
            i.SignIn.RequireConfirmedAccount = false;
            i.SignIn.RequireConfirmedEmail = true;
            i.SignIn.RequireConfirmedPhoneNumber = false;
            //i.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";

            i.User.RequireUniqueEmail = true;

            //c.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmPOIUYTREWQLKJHGFDSAMNBVCXZ";
            i.Password.RequireDigit = false;
            i.Password.RequireNonAlphanumeric = false;
            i.Password.RequireLowercase = false;
            i.Password.RequireUppercase = false;
            i.Password.RequiredUniqueChars = 1;
            i.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<AppIdentityDbContext>();

        services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                        .AddEntityFrameworkStores<AppIdentityDbContext>()
                        .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

        services.AddScoped<IPasswordValidator<ApplicationUser>, LIPasswordValidator>();
        services.AddScoped<IUserValidator<ApplicationUser>, LIUserValidator>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();

        services.AddDbContext<AppIdentityDbContext>(c =>
            c.UseSqlServer(configuration.GetConnectionString("AAA")));

    }

    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                //    c =>
                //    {
                //        c.LoginPath = new PathString("");
                //        c.LogoutPath = new PathString("");
                //        c.AccessDeniedPath = new PathString("");
                //    })
                .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = false,
                            ValidateIssuerSigningKey = true
                        };
                    });
        services.AddAuthorization();
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("GetAllUser",
        //        policy => policy.RequireClaim("AccessAllUser", "True")) ;
        //});

        //services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
    }

    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options => 
        options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"))
        );
    }

    public static void CreateIdentityDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        dataContext.Database.EnsureCreated();
        //dataContext.Database.Migrate();
    }
}
