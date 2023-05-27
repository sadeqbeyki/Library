using LMS.Contracts.Book;

namespace LMS.Contracts.Author;

public interface IAuthorService
{
    Task<AuthorDto> GetById(Guid id);
    Task<List<AuthorDto>> GetAll();
    Task<AuthorDto> Create(AuthorDto entity);
    Task Delete(Guid id);
    Task<AuthorDto> Update(Guid id, AuthorDto entity);

    Task<List<BookDto>> GetAuthorBooks(Guid id);
}
