using AppFramework.Domain;
using LMS.Domain.BookAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
