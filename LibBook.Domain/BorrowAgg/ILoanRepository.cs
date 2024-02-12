using AppFramework.Domain;
using LibBook.DomainContracts.Borrow;

namespace LibBook.Domain.BorrowAgg;

public interface ILoanRepository : IRepository<Borrow, int>
{
    Task<List<LoanDto>> GetBorrowsByMemberId(Guid memberId);
    Task<List<LoanDto>> GetMemberOverdueLoans(Guid memberId);
    Task<List<LoanDto>> GetDuplicatedLoans(Guid memberId, int bookId);
    Task<List<LoanDto>> GetBorrowsByEmployeeId(Guid EmployeeId);
    List<LoanDto> Search(LoanSearchModel searchModel);
    Task<string> GetReturnEmployeeName(Guid? id);
}
