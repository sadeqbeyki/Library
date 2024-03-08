using Identity.ACL.Identity;
using Library.Application.ACLs;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.ACL;

public static class ServiceExtentions
{
    public static void AddIdentityACL(this IServiceCollection services)
    {
        services.AddScoped<IIdentityAcl, IdentityAcl>();
    }
}
