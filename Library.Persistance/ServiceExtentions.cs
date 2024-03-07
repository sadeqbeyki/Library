using Microsoft.Extensions.DependencyInjection;

namespace Library.Persistance;

public static class ServiceExtentions
{
    public static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        return services;
    }
}
