using AppFramework.Domain;
using LMS.Contracts.Author;
using LMS.Domain.BookAgg;

namespace LMS.Domain.AuthorAgg
{
    public interface IAuthorRepository : IRepository<Author, int>
    {
        Task<List<Book>> GetAuthorBooks(int id);
        Task<List<AuthorDto>> GetAuthors();


    }
}
