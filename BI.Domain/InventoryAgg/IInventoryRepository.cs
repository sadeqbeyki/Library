using AppFramework.Domain;
using BI.ApplicationContracts.Inventory;

namespace BI.Domain.InventoryAgg
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        EditInventory GetDetails(long id);
        Inventory GetBy(Guid bookId);
        List<InventoryViewModel> Search(InventorySearchModel searchModel);
        List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
    }
}
