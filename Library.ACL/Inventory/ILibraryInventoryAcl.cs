using Library.Domain.Entities.LendAgg;

namespace Library.ACL.Inventory;

public interface ILibraryInventoryAcl
{
    //bool LendFromInventory(List<LendItem> item);
    bool BorrowFromInventory(Lend lend);
    bool ReturnToInventory(Lend lend);
}