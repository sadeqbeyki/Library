using AppFramework.Application;

namespace BI.ApplicationContracts.Inventory
{
    public interface IInventoryApplication
    {
        OperationResult Create(CreateInventory command);
        Task<OperationResult> Edit(EditInventory command);
        Task<OperationResult> Increase(IncreaseInventory command);
        Task<OperationResult> Reduce(ReduceInventory command);
        OperationResult Reduce(List<ReduceInventory> command);
        EditInventory GetDetails(Guid id);
        List<InventoryViewModel> Search(InventorySearchModel searchModel);
        List<InventoryOperationViewModel> GetOperationLog(Guid inventoryId);
    }
}
