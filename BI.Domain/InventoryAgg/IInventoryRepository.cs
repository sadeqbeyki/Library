using AppFramework.Domain;
using BI.ApplicationContracts.Inventory;

namespace BI.Domain.InventoryAgg
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        EditInventory GetDetails(Guid id);
        Inventory GetBookBy(Guid bookId);
        List<InventoryViewModel> Search(InventorySearchModel searchModel);
        List<InventoryOperationViewModel> GetOperationLog(Guid inventoryId);
    }
}
