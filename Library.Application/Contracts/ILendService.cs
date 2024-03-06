using AppFramework.Application;
using Library.Application.DTOs.Lend;

namespace Library.Application.Contracts;

public interface ILendService
{
    List<LendDto> GetAll();
    List<LendDto> Search(LendSearchModel searchModel);

    Task<List<LendDto>> GetPendingLoans();
    Task<List<LendDto>> GetApprovedLoans();
    Task<List<LendDto>> GetReturnedLoans();
    Task<List<LendDto>> GetDeletedLoans();

    Task<LendDto> GetLendById(int lendId);
    Task<List<LendDto>> GetLoansByMemberId(Guid memberId);
    Task<List<LendDto>> GetLoansByEmployeeId(Guid EmployeeId);

    Task<List<LendDto>> GetOverdueLones();

    OperationResult Update(LendDto dto);
    Task Delete(int lendId);
    void SoftDelete(LendDto entity);

    Task<OperationResult> Lending(LendDto dto);
    Task<OperationResult> SubmitLend(int lendId);

    OperationResult Returning(LendDto dto);
}
