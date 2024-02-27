using Library.ACL.Identity;
using Library.ACL.Inventory;
using Microsoft.Extensions.DependencyInjection;

namespace Library.ACL;

public static class ServiceExtentions
{
    public static void AddACLConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ILibraryInventoryAcl, LibraryInventoryAcl>();
        services.AddScoped<ILibraryIdentityAcl, LibraryIdentityAcl>();
    }
}
