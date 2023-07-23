using AppFramework.Domain;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.BookCategory;

namespace LibBook.Domain.BookCategoryAgg;

public interface IBookCategoryRepository : IRepository<BookCategory, int>
{
    Task<List<Book>> GetCategoryWithBooks(int id);
    Task<List<BookCategoryDto>> GetCategories();
}
