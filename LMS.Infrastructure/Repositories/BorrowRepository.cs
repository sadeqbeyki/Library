using AppFramework.Domain;
using LMS.Domain.Borrow;

namespace LMS.Infrastructure.Repositories;

public class BorrowRepository : Repository<Borrow>, IBorrowRepository
{
    private readonly BookDbContext _bookDbContext;
    public BorrowRepository(BookDbContext dbContext) : base(dbContext)
    {
        _bookDbContext = dbContext;
    }
}
