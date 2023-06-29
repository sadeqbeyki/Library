using AppFramework.Domain;
using LMS.Contracts.Loan;

namespace LMS.Domain.LoanAgg;

public interface ILoanRepository : IRepository<Loan>
{
    Task<IEnumerable<LoanDto>> GetByIdAsync(Func<object, bool> value);
}
