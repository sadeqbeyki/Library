using AppFramework.Application;

namespace LibBook.DomainContracts.Book;

public interface IBookService
{
    Task<UpdateBookViewModel> GetById(int id);
    //Task<List<BookViewModel>> GetAll();
    Task<List<BookViewModel>> GetBooks();
    Task<List<BookViewModel>> GetAllBooks();
    Task<OperationResult> Create(CreateBookViewModel bookDto);
    Task Delete(int id);
    Task<OperationResult> Update(UpdateBookViewModel dto);


}
