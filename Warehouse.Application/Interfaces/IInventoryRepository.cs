using AppFramework.Domain;
using Warehouse.Application.DTOs;
using Warehouse.Domain.Entities.InventoryAgg;

namespace Warehouse.Application.Contracts;

public interface IInventoryRepository : IRepository<Inventory, int>
{
    EditInventory GetDetails(int id);
    Inventory GetBy(int bookId);
    List<InventoryViewModel> Search(InventorySearchModel searchModel);
    List<InventoryOperationViewModel> GetOperationLog(int inventoryId);
}
