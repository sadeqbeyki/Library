using AppFramework.Application;
using Library.Application.DTOs.Book;

namespace Library.Application.Contracts;

public interface IBookService
{
    Task<BookViewModel> GetById(int id);
    Task<List<BookViewModel>> GetAll();
    Task<List<BookViewModel>> GetBooks();

    List<BookViewModel> Search(BookSearchModel searchModel);

    Task<OperationResult> Create(CreateBookModel createModel);
    Task Delete(int id);
    Task<OperationResult> Update(BookViewModel model);
}
