using AppFramework.Domain;
using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.Borrow;

namespace LibBook.Domain.BorrowAgg;

public interface ILoanRepository : IRepository<Borrow, int>
{
    Task<List<LoanDto>> GetBorrowsByMemberId(string memberId);
    Task<List<LoanDto>> GetMemberOverdueLoans(string memberId);
    Task<List<LoanDto>> GetDuplicatedLoans(string memberId, int bookId);
    Task<List<LoanDto>> GetBorrowsByEmployeeId(string EmployeeId);
    List<LoanDto> Search(LoanSearchModel searchModel);
}
