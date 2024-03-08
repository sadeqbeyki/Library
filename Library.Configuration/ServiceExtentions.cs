using Library.Application.Contracts;
using Library.Application.Interfaces;
using Library.Persistance;
using Library.Persistance.Repositories;
using Library.Persistance.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class ServiceExtentions
{
    public static void AddBookInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
        services.AddScoped<IBookCategoryService, BookCategoryService>();

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookService, BookService>();

        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IPublisherService, PublisherService>();

        services.AddScoped<IAuthorRepository, AuthorRepository>();

        services.AddScoped<ITranslatorRepository, TranslatorRepository>();
        services.AddScoped<ITranslatorService, TranslatorService>();

        services.AddScoped<ILendRepository, LendRepository>();

        services.AddDbContext<BookDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
    public static void CreateBookDatabase(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<BookDbContext>();
        dataContext.Database.EnsureCreated();
    }
}
