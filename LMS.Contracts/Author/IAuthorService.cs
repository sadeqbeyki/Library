using LMS.Contracts.Book;

namespace LMS.Contracts.Author;

public interface IAuthorService
{
    Task<AuthorDto> GetById(int id);
    Task<List<AuthorDto>> GetAll();
    Task<AuthorDto> Create(AuthorDto entity);
    Task Delete(int id);
    Task<AuthorDto> Update(int id, AuthorDto entity);

    Task<List<BookDto>> GetAuthorBooks(int id);
    Task<List<AuthorDto>> GetAuthors();

}
