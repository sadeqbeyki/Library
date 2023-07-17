using AppFramework.Domain;
using LMS.Contracts.Lend;

namespace LMS.Domain.LendAgg;

public interface ILendRepository : IRepository<Lend>
{
    Lend GetBookBy(Guid bookId);
    List<LendItemDto> GetItems(Guid lendId);
    List<LendDto> Search(LendSearchModel searchModel);
}
