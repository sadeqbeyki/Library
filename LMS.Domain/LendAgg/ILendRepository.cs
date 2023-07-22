using AppFramework.Application;
using AppFramework.Domain;
using LMS.Contracts.Lend;

namespace LMS.Domain.LendAgg;

public interface ILendRepository : IRepository<Lend, int>
{
    //Lend GetBookBy(int bookId);
    List<LendItemDto> GetItems(int lendId);
    List<LendDto> Search(LendSearchModel searchModel);
}
