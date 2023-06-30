using AppFramework.Domain;

namespace LMS.Domain.OrderAgg;

public interface IRentRepository : IRepository<Rent>
{
    double GetAmountBy(long id);
    List<OrderItemViewModel> GetItems(long orderId);
    List<OrderViewModel> Search(OrderSearchModel searchModel);
}