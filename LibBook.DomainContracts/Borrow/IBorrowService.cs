using AppFramework.Application;

namespace LibBook.DomainContracts.Borrow;

public interface IBorrowService
{
    Task<List<BorrowDto>> GetAll();
    Task<List<BorrowDto>> GetAllBorrows();
    Task<BorrowDto> GetBorrowById(int id);
    Task<IEnumerable<BorrowDto>> GetBorrowsByMemberId(string memberId);
    Task<IEnumerable<BorrowDto>> GetBorrowsByEmployeeId(string employeeId);
    Task<IEnumerable<BorrowDto>> GetOverdueBorrows();

    Task<OperationResult> Update(BorrowDto dto);
    Task Delete(int borrowId);

    Task<OperationResult> Borrowing(BorrowDto dto);
    Task<OperationResult> BorrowingRegistration(int borrowId);
}
