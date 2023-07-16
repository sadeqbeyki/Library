using AppFramework.Domain;
using LendBook.ApplicationContract.Rent;

namespace LendBook.Domain.RentAgg;

public interface IRentRepository : IRepository<Rent>
{
    double GetAmountBy(Guid id);
    List<RentItemViewModel> GetItems(Guid rentId);
    List<RentViewModel> Search(RentSearchModel searchModel);
}