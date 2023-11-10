using AppFramework.Domain;
using LibBook.DomainContracts.Borrow;

namespace LibBook.Domain.BorrowAgg;

public interface ILoanRepository : IRepository<Borrow, int>
{
    Task<List<LoanDto>> GetBorrowsByMemberId(string memberId);
    Task<List<LoanDto>> GetBorrowsByEmployeeId(string EmployeeId);
    Task<List<LoanDto>> GetOverdueLones();
}
