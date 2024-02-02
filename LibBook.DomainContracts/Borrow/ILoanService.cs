using AppFramework.Application;

namespace LibBook.DomainContracts.Borrow;

public interface ILoanService
{
    List<LoanDto> GetAll();
    List<LoanDto> Search(LoanSearchModel searchModel);

    Task<List<LoanDto>> GetPendingLoans();
    List<LoanDto> GetApprovedLoans();
    List<LoanDto> GetReturnedLoans();
    List<LoanDto> GetDeletedLoans();

    Task<LoanDto> GetLoanById(int borrowId);
    Task<List<LoanDto>> GetBorrowsByMemberId(Guid memberId);
    Task<List<LoanDto>> GetBorrowsByEmployeeId(Guid EmployeeId);

    Task<List<LoanDto>> GetOverdueLones();

    OperationResult Update(LoanDto dto);
    Task Delete(int borrowId);
    void SoftDelete(LoanDto entity);

    Task<OperationResult> Lending(LoanDto dto);
    Task<OperationResult> SubmitLend(int borrowId);

    OperationResult Returning(LoanDto dto);
}
