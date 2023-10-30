using AppFramework.Domain;
using LibBook.DomainContracts.Borrow;

namespace LibBook.Domain.BorrowAgg;

public interface IBorrowRepository : IRepository<Borrow, int>
{
    Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId);
    Task<List<BorrowDto>> GetBorrowsByEmployeeId(string EmployeeId);
    Task<List<BorrowDto>> GetOverdueBorrows();
}
