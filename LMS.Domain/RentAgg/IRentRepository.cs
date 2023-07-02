using AppFramework.Domain;
using LMS.Contracts.Rent;

namespace LMS.Domain.OrderAgg;

public interface IRentRepository : IRepository<Rent>
{
    double GetAmountBy(long id);
    List<RentItemViewModel> GetItems(long rentId);
    List<RentViewModel> Search(RentSearchModel searchModel);
}