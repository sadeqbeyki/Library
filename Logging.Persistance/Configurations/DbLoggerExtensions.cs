using Logging.Persistance.Domain;
using Logging.Persistance.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Logging.Persistance.Configurations;

public static class DbLoggerExtensions
{
    public static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder, Action<DbLoggerOptions> configure)
    {
        builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
        builder.Services.Configure(configure);
        return builder;
    }

    //public static void CreateIdentityDatabase(this IApplicationBuilder app)
    //{
    //    using var serviceScope = app.ApplicationServices.CreateScope();
    //    var dataContext = serviceScope.ServiceProvider.GetRequiredService<DbLogger>();
    //    dataContext.Database.EnsureCreated();
    //}
}
