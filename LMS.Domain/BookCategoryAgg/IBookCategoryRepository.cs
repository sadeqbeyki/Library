using AppFramework.Domain;
using LMS.Contracts.BookCategoryContract;
using LMS.Domain.BookAgg;

namespace LMS.Domain.BookCategoryAgg;

public interface IBookCategoryRepository : IRepository<BookCategory>
{
    Task<List<Book>> GetCategoryWithBooks(Guid id);
    Task<List<BookCategoryDto>> GetCategories();
}
