using AppFramework.Domain;
using LMS.Domain.AuthorAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(BookDbContext dbContext) : base(dbContext)
        {
        }
    }
}
