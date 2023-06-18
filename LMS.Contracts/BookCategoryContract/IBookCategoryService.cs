using AppFramework.Application;
using LMS.Contracts.Book;

namespace LMS.Contracts.BookCategoryContract;

public interface IBookCategoryService
{
    Task<BookCategoryDto> GetById(Guid id);
    Task<List<BookCategoryDto>> GetAll();
    Task<OperationResult> Create(BookCategoryDto entity);
    Task Delete(Guid id);
    Task<OperationResult> Update(Guid id, BookCategoryDto entity);
    Task<List<BookDto>> GetCategoryWithBooks(Guid id);

}
