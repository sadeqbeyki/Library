using AppFramework.Application;

namespace LMS.Contracts.Loan;

public interface ILendService
{
    Task<List<LendDto>> GetAllLends();
    Task<LendDto> GetLendById(Guid id);
    Task<IEnumerable<LendDto>> GetLendsByMemberId(string memberId);
    Task<IEnumerable<LendDto>> GetLendsByEmployeeId(string employeeId);
    Task<IEnumerable<LendDto>> GetOverdueLends();
    Task<LendDto> Create(LendDto loan);
    Task<OperationResult> Update(LendDto loan);
    Task Delete(Guid id);
}
