using AppFramework.Application.Email;
using LibBook.Domain;
using LibBook.Configurations;
using LibInventory.Configuration;
using Identity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
     //need for component
     .AddRazorOptions(options =>
     {
         options.ViewLocationFormats.Add("/{0}.cshtml");
     });

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

#region token
builder.Services.AddJwtAuth(builder.Configuration);
// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
//Jwt configuration ends here
#endregion end token

#region Email
builder.Services.AddTransient<IEmailService, EmailService>();
#endregion


#region DependencyInjection

builder.Services.AddInventoryInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddBookInfrastructure(builder.Configuration);

#endregion


var app = builder.Build();

#region CreateDbWhenDosentExist
app.CreateBookDatabase();
app.CreateInventoryDatabase();
app.CreateIdentityDatabase();
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

app.UseAuthentication();

app.UseRouting();

app.UseSession();

app.UseAuthorization();
//IConfiguration configuration = app.Configuration;
//IWebHostEnvironment environment = app.Environment;


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

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
