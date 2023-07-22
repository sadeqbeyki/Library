using AppFramework.Domain;
using LMS.Domain.BorrowAgg;

namespace LMS.Infrastructure.Repositories;

public class BorrowRepository : Repository<Borrow, int>, IBorrowRepository
{
    private readonly BookDbContext _bookDbContext;
    public BorrowRepository(BookDbContext dbContext) : base(dbContext)
    {
        _bookDbContext = dbContext;
    }
}
