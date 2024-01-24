using LibBook.ApplicationServices;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.Domain.BookCategoryAgg;
using LibBook.Domain.BorrowAgg;
using LibBook.Domain.PublisherAgg;
using LibBook.Domain.Services;
using LibBook.Domain.TranslatorAgg;
using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Borrow;
using LibBook.DomainContracts.Publisher;
using LibBook.DomainContracts.Translator;
using LibBook.Infrastructure;
using LibBook.Infrastructure.AccountACL;
using LibBook.Infrastructure.InventoryACL;
using LibBook.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibBook.Configurations;

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
        services.AddScoped<IAuthorService, AuthorService>();

        services.AddScoped<ITranslatorRepository, TranslatorRepository>();
        services.AddScoped<ITranslatorService, TranslatorService>();

        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ILoanService, LoanService>();

        services.AddScoped<ILibraryInventoryAcl, LibraryInventoryAcl>();
        services.AddScoped<ILibraryIdentityAcl, LibraryIdentityAcl>();

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
