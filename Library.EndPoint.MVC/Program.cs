using AppFramework.Application.Email;
using Identity.Application;
using Identity.Services;
using Library.Infrastructure;
using Logging.Persistance.Configurations;
using Warehouse.Configuration;
using Warehouse.Application;
using Library.Application;
using Identity.ACL;
using Inventory.ACL;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

//Logging

builder.Logging.AddDbLogger(options =>
{
    builder.Configuration.GetSection("Logging").GetSection("Database").GetSection("Options").Bind(options);
});

// Add services to the container.
builder.Services.AddControllersWithViews()
     //need for component (OverdueCountViewComponent.cshtml)
     .AddRazorOptions(options =>
     {
         options.ViewLocationFormats.Add("/{0}.cshtml");
     });

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

#region Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisCacheUrl");
    options.InstanceName = "RedisInstance_";
});
#endregion


#region token
builder.Services.AddJwtAuth(builder.Configuration);
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
#endregion end token




#region DependencyInjection
// -------------------- AutoMapper --------------------
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});
// -------------------- AutoMapper --------------------

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

builder.Services.AddLibraryApplications();

builder.Services.AddInventoryInfrastructure(builder.Configuration);
builder.Services.AddBookInfrastructure(builder.Configuration);
builder.Services.AddIdentityACL();
builder.Services.AddInventoryACL();
builder.Services.AddInventoryApplication();
#endregion


var app = builder.Build();

#region CreateDbWhenDosentExist
//app.CreateBookDatabase();
//app.CreateInventoryDatabase();
//app.CreateIdentityDatabase();
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// app.UseCookiePolicy();

app.UseRouting();
// app.UseRateLimiter();
// app.UseRequestLocalization();
// app.UseCors();

//app.UseStatusCodePages(async context =>
//{
//    var response = context.HttpContext.Response;
//    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
//    {
//        response.Redirect("/AccessDenied");
//    }
//});

app.UseAuthentication();
app.UseAuthorization();
// app.UseResponseCompression();
// app.UseResponseCaching();
app.UseSession();
// app.UseResponseCompression();
// app.UseResponseCaching();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
      name: "adminPanel",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    _ = endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
