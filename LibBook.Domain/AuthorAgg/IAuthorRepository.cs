using AppFramework.Domain;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Author;

namespace LibBook.Domain.AuthorAgg
{
    public interface IAuthorRepository : IRepository<Author, int>
    {
        Task<List<Book>> GetAuthorBooks(int id);
        Task<List<AuthorDto>> GetAuthors();

        Task<Author?> GetByName(string name);

    }
}
