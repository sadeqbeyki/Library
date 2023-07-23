using AppFramework.Application;

namespace LibBook.DomainContracts.Lend;

public interface ILendService
{
    Task<List<LendDto>> GetAllLends();
    Task<LendDto> GetLendById(int id);
    Task<IEnumerable<LendDto>> GetLendsByMemberId(string memberId);
    Task<IEnumerable<LendDto>> GetLendsByEmployeeId(string employeeId);
    Task<IEnumerable<LendDto>> GetOverdueLends();
    Task<OperationResult> Update(LendDto lend);
    Task Delete(int id);

    //Task<OperationResult> Create(LendDto dto);
    Task<OperationResult> Lending(LendDto dto);
    Task<OperationResult> LendingRegistration(int lendId);
    List<LendItemDto> GetItems(int lendId);
    List<LendDto> Search(LendSearchModel searchModel);
}
