using AppFramework.Domain;
using LibBook.Domain.BorrowAgg;

namespace LibBook.Infrastructure.Repositories;

public class BorrowRepository : Repository<Borrow, int>, IBorrowRepository
{
    private readonly BookDbContext _bookDbContext;
    public BorrowRepository(BookDbContext dbContext) : base(dbContext)
    {
        _bookDbContext = dbContext;
    }
}
