using AppFramework.Domain;
using LMS.Contracts.Loan;
using LMS.Domain.LendAgg;

namespace LMS.Infrastructure.Repositories;

public class LendRepository : Repository<Lend>, ILendRepository
{
    private readonly BookDbContext _dbContext;
    public LendRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

}
