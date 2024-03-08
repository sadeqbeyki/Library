using Inventory.ACL.Inventory;
using Library.Application.ACLs;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.ACL;

public static class ServiceExtentions
{
    public static void AddInventoryACL(this IServiceCollection services)
    {
        services.AddScoped<ILibraryInventoryAcl, LibraryInventoryAcl>();
    }
}
