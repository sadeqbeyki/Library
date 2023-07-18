using LMS.Domain.LendAgg;

namespace LMS.Domain.Services;

public interface ILibraryInventoryAcl
{
    bool LendFromInventory(List<LendItem> item);
}