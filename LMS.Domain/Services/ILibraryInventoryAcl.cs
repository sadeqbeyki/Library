using LMS.Domain.LendAgg;

namespace LMS.Domain.Services;

public interface ILibraryInventoryAcl
{
    bool DecreaseFromInventory(List<LendItem> items);
    bool LendFromInventory(List<LendItem> item);
}