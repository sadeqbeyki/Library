using LMS.Contracts.Book;

namespace LMS.Contracts.BookCategory;

public interface IBookCategoryService
{
    Task<BookCategoryDto> GetById(Guid id);
    Task<List<BookCategoryDto>> GetAll();
    Task<BookCategoryDto> Create(BookCategoryDto entity);
    Task Delete(Guid id);
    Task<BookCategoryDto> Update(Guid id, BookCategoryDto entity);
    Task<List<BookDto>> GetCategoryWithBooks(Guid id);

}
