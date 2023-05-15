using AppFramework.Domain;
using LMS.Domain.AuthorAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class AutherRepository : Repository<Author>, IAutherRepository
    {
        public AutherRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
