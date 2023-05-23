namespace LMS.Contracts.Book;

public interface IBookService
{
    Task<BookDto> GetById(Guid id);
    Task<List<BookDto>> GetAll();
    Task<BookDto> Create(BookDto entity);
    Task Delete(Guid id);
    Task<BookDto> Update(Guid id, BookDto entity);
}
