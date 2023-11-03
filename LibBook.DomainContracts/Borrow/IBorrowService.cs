using AppFramework.Application;

namespace LibBook.DomainContracts.Borrow;

public interface IBorrowService
{
    Task<List<BorrowDto>> GetAll();
    Task<List<BorrowDto>> GetAllLoans();
    List<BorrowDto> GetApprovedLoans();
    List<BorrowDto> GetReturnedLoans();

    Task<BorrowDto> GetBorrowById(int borrowId);
    Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId);
    Task<List<BorrowDto>> GetBorrowsByEmployeeId(string EmployeeId);

    Task<List<BorrowDto>> GetOverdueBorrows();

    OperationResult Update(BorrowDto dto);
    Task Delete(int borrowId);
    Task SoftDeleteAsync(BorrowDto entity);

    Task<OperationResult> Lending(BorrowDto dto);
    Task<OperationResult> SubmitLend(int borrowId);

    OperationResult Returning(BorrowDto dto);
}
