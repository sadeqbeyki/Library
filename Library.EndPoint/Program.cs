using BI.Configuration;
using BMS.Configurations;
using LI.ApplicationContracts.Auth;
using LI.ApplicationContracts.UserContracts;
using LI.ApplicationServices;
using LI.Domain.RoleAgg;
using LI.Domain.UserAgg;
using LI.Infrastructure;
using LI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();

#region Book
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
BMSConfigureServices.Configure(builder.Services, connectionString);
InventoryConfigureServices.Configure(builder.Services, connectionString);
#endregion

#region Identity
builder.Services.AddIdentity<User, Role>(i =>
{
    i.User.RequireUniqueEmail = true;
    //c.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmPOIUYTREWQLKJHGFDSAMNBVCXZ";
    i.Password.RequireDigit = false;
    i.Password.RequiredLength = 6;
    i.Password.RequireNonAlphanumeric = false;
    i.Password.RequireUppercase = false;
    i.Password.RequiredUniqueChars = 1;
    i.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<LiIdentityDbContext>();

builder.Services.AddScoped<IPasswordValidator<User>, LIPasswordValidator>();
builder.Services.AddScoped<IUserValidator<User>, LIUserValidator>();
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<LiIdentityDbContext>(c =>
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

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");



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
