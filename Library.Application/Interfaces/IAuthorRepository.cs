using AppFramework.Domain;
using Library.Application.DTOs.Author;
using Library.Domain.Entities.AuthorAgg;
using Library.Domain.Entities.BookAgg;

namespace Library.Application.Interfaces;

public interface IAuthorRepository : IRepository<Author, int>
{
    Task<List<Book>> GetAuthorBooks(int id);
    Task<List<AuthorDto>> GetAuthors();

    Task<Author?> GetByName(string name);

}
