using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Warehouse.Application;

public static class ServiceExtentions
{
    public static void AddInventoryApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    }
}
