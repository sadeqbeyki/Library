using LibBook.Configurations;
using LibIdentity.ApplicationServices;
using LibIdentity.Domain.RoleAgg;
using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.Auth;
using LibIdentity.DomainContracts.RoleContracts;
using LibIdentity.DomainContracts.UserContracts;
using LibIdentity.Infrastructure;
using LibIdentity.Infrastructure.Repositories;
using LibInventory.Configuration;
using Library.EndPoint.Areas.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

#region Email

#endregion


#region Book
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
LMSConfigureServices.Configure(builder.Services, connectionString);
InventoryConfigureServices.Configure(builder.Services, connectionString);
#endregion

#region Identity
builder.Services.AddIdentity<User, Role>(i =>
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
})
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddScoped<IPasswordValidator<User>, LIPasswordValidator>();
builder.Services.AddScoped<IUserValidator<User>, LIUserValidator>();
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
//builder.Services.AddScoped<IUserManager, UserManager>();
//builder.Services.AddScoped<ISignInManager, SignInManager>();

builder.Services.AddDbContext<IdentityDbContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("AAA")));
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseSession();

app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "adminPanel",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
