using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Application.Contracts;
using Warehouse.Persistance;
using Warehouse.Persistance.Repositories;
using Warehouse.Service.Contracts;
using Warehouse.Service.Services;

namespace Warehouse.Configuration;

public static class ServiceExtentions
{
    public static void AddInventoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IInventoryService, InventoryService>();
        services.AddTransient<IInventoryRepository, InventoryRepository>();


        //services.AddTransient<IPermissionExposer, InventoryPermissionExposer>();
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
    public static void CreateInventoryDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        dataContext.Database.EnsureCreated();
    }
}
