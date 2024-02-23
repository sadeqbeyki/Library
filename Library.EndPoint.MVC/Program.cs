using AppFramework.Application.Email;
using Identity.Application;
using Identity.Services;
using LibBook.Configurations;
using LibInventory.Configuration;
using Logging.Persistance.Configurations;


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
builder.Services.AddAutoMapper(typeof(IdentityMapProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddInventoryInfrastructure(builder.Configuration);
builder.Services.AddBookInfrastructure(builder.Configuration);
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
