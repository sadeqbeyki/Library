using AppFramework.Domain;
using LibBook.DomainContracts.Lend;

namespace LibBook.Domain.LendAgg;

public interface ILendRepository : IRepository<Lend, int>
{
    //Lend GetBookBy(int bookId);
    List<LendItemDto> GetItems(int lendId);
    List<LendDto> Search(LendSearchModel searchModel);
}
