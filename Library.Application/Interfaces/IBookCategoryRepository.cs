using AppFramework.Domain;
using Library.Application.DTOs.BookCategories;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.BookCategoryAgg;

namespace Library.Application.Interfaces;

public interface IBookCategoryRepository : IRepository<BookCategory, int>
{
    Task<List<Book>> GetCategoryWithBooks(int id);
    Task<List<BookCategoryDto>> GetCategories();
}
