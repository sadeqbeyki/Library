using Library.Application.DTOs.Author;
using Library.Application.DTOs.Book;

namespace Library.Application.Contracts;

public interface IAuthorService
{
    Task<AuthorDto> GetById(int id);
    Task<List<AuthorDto>> GetAll();
    Task<AuthorDto> Create(AuthorDto entity);
    Task Delete(int id);
    Task<AuthorDto> Update(int id, AuthorDto entity);

    Task<List<BookViewModel>> GetAuthorBooks(int id);
    Task<List<AuthorDto>> GetAuthors();

}
