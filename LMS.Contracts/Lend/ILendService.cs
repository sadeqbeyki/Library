using AppFramework.Application;

namespace LMS.Contracts.Lend;

public interface ILendService
{
    Task<List<LendDto>> GetAllLends();
    Task<LendDto> GetLendById(Guid id);
    Task<IEnumerable<LendDto>> GetLendsByMemberId(string memberId);
    Task<IEnumerable<LendDto>> GetLendsByEmployeeId(string employeeId);
    Task<IEnumerable<LendDto>> GetOverdueLends();
    Task<LendDto> Create(LendDto lend);
    Task<OperationResult> Update(LendDto lend);
    Task Delete(Guid id);

    long PlaceOrder(Cart cart);
    string PaymentSucceeded(long orderId, long refId);
    List<LendItemDto> GetItems(long orderId);
    List<LendDto> Search(LendSearchModel searchModel);
}
