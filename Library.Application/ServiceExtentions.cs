using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Library.Application;

public static class ServiceExtentions
{
    public static void AddLibraryApplications(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
