using LibInventory.ApplicationServices;
using LibInventory.Domain.InventoryAgg;
using LibInventory.DomainContracts.Inventory;
using LibInventory.Infrastructure;
using LibInventory.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibInventory.Configuration;

public static class ServiceExtentions
{
    public static void AddInventoryInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IInventoryService, InventoryService>();
        services.AddTransient<IInventoryRepository, InventoryRepository>();


        //services.AddTransient<IPermissionExposer, InventoryPermissionExposer>();
        services.AddDbContext<InventoryDbContext>(options=>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
    public static void CreateInventoryDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        dataContext.Database.EnsureCreated();
    }
}
