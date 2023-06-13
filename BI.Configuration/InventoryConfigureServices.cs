using BI.ApplicationContracts.Inventory;
using BI.ApplicationServices;
using BI.Domain.InventoryAgg;
using BI.Infrastructure;
using BI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BI.Configuration;

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
