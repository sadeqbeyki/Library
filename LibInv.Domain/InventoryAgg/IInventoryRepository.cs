using AppFramework.Domain;
using LibInventory.DomainContracts.Inventory;

namespace LibInventory.Domain.InventoryAgg;

public interface IInventoryRepository : IRepository<Inventory, int>
{
    EditInventory GetDetails(int id);
    Inventory GetBy(int bookId);
    List<InventoryViewModel> Search(InventorySearchModel searchModel);
    List<InventoryOperationViewModel> GetOperationLog(int inventoryId);
}
