using AppFramework.Application;
using Warehouse.Application.DTOs;
using Warehouse.Application.DTOs.Inventory;
using Warehouse.Application.DTOs.InventoryOperation;

namespace Warehouse.Service.Contracts;

public interface IInventoryService
{
    Task<OperationResult> Create(CreateInventory command);
    Task<OperationResult> Edit(EditInventory command);
    Task<OperationResult> Increase(IncreaseInventory command);
    Task<OperationResult> Decrease(DecreaseInventory command);
    OperationResult Decrease(List<DecreaseInventory> command);
    EditInventory GetDetails(int id);
    List<InventoryViewModel> Search(InventorySearchModel searchModel);
    List<InventoryOperationViewModel> GetOperationLog(int inventoryId);

    OperationResult Lending(DecreaseInventory command);
    OperationResult Returning(ReturnBook command);
}
