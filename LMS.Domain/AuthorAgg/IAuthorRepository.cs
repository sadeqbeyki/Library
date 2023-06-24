using AppFramework.Domain;
using LMS.Contracts.Author;
using LMS.Contracts.BookCategoryContract;
using LMS.Domain.BookAgg;

namespace LMS.Domain.AuthorAgg
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<List<Book>> GetAuthorBooks(Guid id);
        Task<List<AuthorDto>> GetAuthors();


    }
}
