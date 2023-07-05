using AppFramework.Domain;
using LMS.Contracts.Rent;

namespace LMS.Domain.RentAgg;

public interface IRentRepository : IRepository<Rent>
{
    double GetAmountBy(Guid id);
    List<RentItemViewModel> GetItems(Guid rentId);
    List<RentViewModel> Search(RentSearchModel searchModel);
}