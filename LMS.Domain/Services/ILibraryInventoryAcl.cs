
using LMS.Domain.OrderAgg;

namespace LMS.Domain.Services;

public interface ILibraryInventoryAcl
{
    bool ReduceFromInventory(List<RentItem> items);
}