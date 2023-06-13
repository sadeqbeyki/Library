namespace LMS.Contracts.Book;

public interface IBookService
{
    Task<BookViewModel> GetById(Guid id);
    Task<List<BookViewModel>> GetAll();
    Task<BookDto> Create(BookDto entity);
    Task Delete(Guid id);
    Task<BookDto> Update(Guid id, BookDto entity);

    Task<List<BookViewModel>> GetBooks();

}
