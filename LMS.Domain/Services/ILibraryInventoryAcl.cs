using LMS.Domain.RentAgg;

namespace LMS.Domain.Services;

public interface ILibraryInventoryAcl
{
    bool DecreaseFromInventory(List<RentItem> items);
}