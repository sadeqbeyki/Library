using AppFramework.Application;
using LMS.Contracts.Book;

namespace LMS.Contracts.BookCategoryContract;

public interface IBookCategoryService
{
    Task<BookCategoryDto> GetById(int id);
    Task<List<BookCategoryDto>> GetAll();
    Task<OperationResult> Create(BookCategoryDto entity);
    Task Delete(int id);
    Task<OperationResult> Update(int id, BookCategoryDto entity);
    Task<List<BookDto>> GetCategoryWithBooks(int id);

    Task<List<BookCategoryDto>> GetCategories();

}
