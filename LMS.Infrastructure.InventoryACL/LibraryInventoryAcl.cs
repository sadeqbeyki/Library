using BI.ApplicationContracts.Inventory;
using LMS.Domain.RentAgg;

namespace LMS.Infrastructure.InventoryACL;

public class LibraryInventoryAcl
{
    private readonly IInventoryService _inventoryService;

    public LibraryInventoryAcl(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public bool ReduceFromInventory(List<RentItem> items)
    {
        var command = items.Select(rentItem =>
                new DecreaseInventory(rentItem.BookId, rentItem.Count, "امانت عضو", rentItem.RentId))
            .ToList();

        return _inventoryService.Decrease(command).IsSucceeded;
    }
}