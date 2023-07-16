using LendBook.Domain.LendAgg;
using LendBook.Domain.RentAgg;

namespace LendBook.Domain.Services;

public interface ILibraryInventoryAcl
{
    bool DecreaseFromInventory(List<RentItem> items);
    bool LendFromInventory(List<LendItem> item);
}