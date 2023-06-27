using AppFramework.Domain;
using LMS.Domain.LoanAgg;

namespace LMS.Infrastructure.Repositories;

public class LoanRepository : Repository<Loan>, ILoanRepository
{
    private readonly BookDbContext _dbContext;
    public LoanRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

}
