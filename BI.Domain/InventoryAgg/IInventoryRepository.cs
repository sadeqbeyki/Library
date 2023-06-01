using AppFramework.Domain;
using InventoryManagement.Application.Contract.Inventory;

namespace BI.Domain.InventoryAgg
{
    public interface IInventoryRepository : IBaseRepository<long, Inventory>
    {
        EditInventory GetDetails(long id);
        Inventory GetBy(long productId);
        List<InventoryViewModel> Search(InventorySearchModel searchModel);
        List<InventoryOperationViewModel> GetOperationLog(long inventoryId);
    }
}
