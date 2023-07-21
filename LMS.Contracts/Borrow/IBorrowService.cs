using AppFramework.Application;

namespace LMS.Contracts.Borrow;

public interface IBorrowService
{
    Task<List<BorrowDto>> GetAll();
    Task<BorrowDto> GetLendById(Guid id);
    Task<IEnumerable<BorrowDto>> GetBorrowsByMemberId(string memberId);
    Task<IEnumerable<BorrowDto>> GetBorrowsByEmployeeId(string employeeId);
    Task<IEnumerable<BorrowDto>> GetOverdueBorrows();

    Task<OperationResult> Update(BorrowDto dto);
    Task Delete(Guid borrowId);

    Task<OperationResult> Borrowing(BorrowDto dto);
    Task<OperationResult> BorrowingRegistration(Guid borrowId);
}
