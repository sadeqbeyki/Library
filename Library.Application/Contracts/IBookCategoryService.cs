using AppFramework.Application;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.BookCategory;

namespace Library.Application.Contracts;

public interface IBookCategoryService
{
    Task<BookCategoryDto> GetById(int id);
    Task<List<BookCategoryDto>> GetCategories();

    Task<OperationResult> Create(BookCategoryDto entity);
    Task<OperationResult> Update(int id, BookCategoryDto entity);
    Task Delete(int id);
    Task<List<BookDto>> GetCategoryWithBooks(int id);

}
