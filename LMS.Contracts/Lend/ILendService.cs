using AppFramework.Application;

namespace LMS.Contracts.Lend;

public interface ILendService
{
    Task<List<LendDto>> GetAllLends();
    Task<LendDto> GetLendById(Guid id);
    Task<IEnumerable<LendDto>> GetLendsByMemberId(string memberId);
    Task<IEnumerable<LendDto>> GetLendsByEmployeeId(string employeeId);
    Task<IEnumerable<LendDto>> GetOverdueLends();
    Task<OperationResult> Update(LendDto lend);
    Task Delete(Guid id);

    //Task<OperationResult> Create(LendDto dto);
    Task<OperationResult> Lending(LendDto dto);
    Task<OperationResult> LendingRegistration(Guid lendId);
    List<LendItemDto> GetItems(Guid lendId);
    List<LendDto> Search(LendSearchModel searchModel);
}
