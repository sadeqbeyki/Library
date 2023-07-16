using AppFramework.Domain;
using LendBook.Domain.LendAgg;

namespace LendBook.Infrastructure.Repositories;

public class LendRepository : Repository<Lend>, ILendRepository
{
    private readonly LendDbContext _dbContext;
    public LendRepository(LendDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Lend GetBookBy(Guid bookId)
    {
        return _dbContext.Lends.FirstOrDefault(b => b.BookId == bookId);
    }
}
