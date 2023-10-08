using AppFramework.Application;

namespace LibBook.DomainContracts.Book;

public interface IBookService
{
    Task<BookViewModel> GetById(int id);
    Task<List<BookViewModel>> GetAll();
    Task<List<BookViewModel>> GetBooks();

    Task<OperationResult> Create(BookDto bookDto);
    Task Delete(int id);
    Task<OperationResult> Update(BookViewModel dto);
}
