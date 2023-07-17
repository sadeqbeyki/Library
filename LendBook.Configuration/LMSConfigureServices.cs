using LendBook.ApplicationContract.Lend;
using LendBook.ApplicationContract.Rent;
using LendBook.ApplicationService;
using LendBook.Domain.LendAgg;
using LendBook.Domain.RentAgg;
using LendBook.Domain.Services;
using LendBook.Infrastructure;
using LendBook.Infrastructure.AccountACL;
using LendBook.Infrastructure.InventoryACL;
using LendBook.Infrastructure.Repositories;
using LMS.Contracts.Lend;
using LMS.Domain.LendAgg;
using LMS.Domain.Services;
using LMS.Infrastructure.Repositories;
using LMS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LendBook.Configuration;

public static class LMSConfigureServices
{
    public static void Configure(IServiceCollection services, string? connectionString)
    {
        services.AddScoped<ILendRepository, LendRepository>();
        services.AddScoped<ILendService, LendService>();

        services.AddTransient<IRentRepository, RentRepository>();
        services.AddTransient<IRentApplication, RentApplication>();

        services.AddSingleton<ICartService, CartService>();
        services.AddTransient<ILibraryInventoryAcl, LibraryInventoryAcl>();
        services.AddTransient<ILibraryAccountAcl, LibraryAccountAcl>();

        services.AddDbContext<LendDbContext>(x => x.UseSqlServer(connectionString));
    }
}
