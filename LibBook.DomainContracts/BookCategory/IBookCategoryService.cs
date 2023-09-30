using AppFramework.Application;
using LibBook.DomainContracts.Book;

namespace LibBook.DomainContracts.BookCategory;

public interface IBookCategoryService
{
    Task<BookCategoryDto> GetById(int id);
    Task<List<BookCategoryDto>> GetAll();
    Task<OperationResult> Create(BookCategoryDto entity);
    Task Delete(int id);
    Task<OperationResult> Update(int id, BookCategoryDto entity);
    Task<List<CreateBookViewModel>> GetCategoryWithBooks(int id);

    Task<List<BookCategoryDto>> GetCategories();

}
