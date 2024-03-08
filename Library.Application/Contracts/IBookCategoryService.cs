using AppFramework.Application;
using Library.Application.DTOs.BookCategory;
using Library.Application.DTOs.Books;

namespace Library.Application.Contracts;

public interface IBookCategoryService
{
    Task<BookCategoryDto> GetById(int id);
    Task<List<BookCategoryDto>> GetCategories();

    Task<OperationResult> Create(BookCategoryDto entity);
    Task<OperationResult> Update(int id, BookCategoryDto entity);
    Task<bool> Delete(int id);
    Task<List<BookDto>> GetCategoryWithBooks(int id);

}
