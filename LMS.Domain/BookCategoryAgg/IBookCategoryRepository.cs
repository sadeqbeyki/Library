using AppFramework.Domain;
using LMS.Contracts.BookCategoryContract;
using LMS.Domain.BookAgg;

namespace LMS.Domain.BookCategoryAgg;

public interface IBookCategoryRepository : IRepository<BookCategory, int>
{
    Task<List<Book>> GetCategoryWithBooks(int id);
    Task<List<BookCategoryDto>> GetCategories();
}
