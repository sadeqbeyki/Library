using AppFramework.Domain;
using LMS.Domain.AuthorAgg;
using LMS.Domain.BookAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly BookDbContext _bookDbContext;
        public AuthorRepository(BookDbContext dbContext) : base(dbContext)
        {
            _bookDbContext = dbContext;
        }

        public async Task<List<Book>> GetAuthorBooks(Guid id)
        {
            List<Book> books = await _bookDbContext.Books.Where(b => b.AuthorId == id).ToListAsync();
            return  books;
        }
    }
}
