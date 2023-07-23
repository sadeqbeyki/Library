using LibInventory.ApplicationServices;
using LibInventory.Domain.InventoryAgg;
using LibInventory.DomainContracts.Inventory;
using LibInventory.Infrastructure;
using LibInventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibInventory.Configuration;

public static class InventoryConfigureServices
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddTransient<IInventoryService, InventoryService>();
        services.AddTransient<IInventoryRepository, InventoryRepository>();


        //services.AddTransient<IPermissionExposer, InventoryPermissionExposer>();

        services.AddDbContext<InventoryDbContext>(x => x.UseSqlServer(connectionString));
    }
}
