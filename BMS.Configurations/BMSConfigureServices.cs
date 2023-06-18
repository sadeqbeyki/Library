using LMS.Contracts.Author;
using LMS.Contracts.Book;
using LMS.Contracts.BookCategoryContract;
using LMS.Contracts.Publisher;
using LMS.Contracts.Translator;
using LMS.Domain.AuthorAgg;
using LMS.Domain.BookAgg;
using LMS.Domain.BookCategoryAgg;
using LMS.Domain.PublisherAgg;
using LMS.Domain.TranslatorAgg;
using LMS.Infrastructure;
using LMS.Infrastructure.Repositories;
using LMS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BMS.Configurations;

public static class BMSConfigureServices
{
    public static void Configure(IServiceCollection services, string? connectionString)
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

        services.AddDbContext<BookDbContext>(x => x.UseSqlServer(connectionString));
    }
}
