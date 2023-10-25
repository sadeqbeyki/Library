using AppFramework.Application;

namespace LibBook.DomainContracts.Borrow;

public interface IBorrowService
{
    Task<List<BorrowDto>> GetAll();
    Task<List<BorrowDto>> GetAllBorrows();
    Task<BorrowDto> GetBorrowById(int borrowId);
    Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId);
    Task<List<BorrowDto>> GetBorrowsByEmployeeId(string EmployeeId);
    Task<List<BorrowDto>> GetOverdueBorrows();

    Task Delete(int borrowId);

    Task<OperationResult> Lending(BorrowDto dto);
    Task<OperationResult> SubmitLend(int borrowId);
    
    Task<OperationResult> Returning(BorrowDto dto);
    //Task<OperationResult> ReturnLoan(int borrowId);
}
