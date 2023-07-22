using AppFramework.Application;

namespace LMS.Contracts.Book;

public interface IBookService
{
    Task<BookViewModel> GetById(int id);
    Task<List<BookViewModel>> GetAll();
    Task<OperationResult> Create(BookDto dto);
    Task Delete(int id);
    Task<OperationResult> Update(BookViewModel dto);

    Task<List<BookViewModel>> GetBooks();

}
