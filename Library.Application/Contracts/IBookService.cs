using Library.Application.DTOs.Books;
using Library.Domain.Entities.BookAgg;

namespace Library.Application.Contracts;

public interface IBookService
{
    Task AddAuthors(BookDto model, Book book);
    Task AddPublishers(BookDto model, Book book);
    Task AddTranslators(BookDto model, Book book);
    Task<byte[]?> ConvertImageToByte(IFormFile Image);
}
