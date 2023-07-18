using BI.ApplicationContracts.Inventory;
using LMS.Domain.LendAgg;
using LMS.Domain.Services;

namespace LMS.Infrastructure.InventoryACL;

public class LibraryInventoryAcl : ILibraryInventoryAcl
{
    private readonly IInventoryService _inventoryService;

    public LibraryInventoryAcl(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public bool LendFromInventory(List<LendItem> items)
    {
        var command = items.Select(l =>
            new DecreaseInventory(
                            l.BookId,
                            1,
                            "Borrow...",
                            l.LendId
                    )).ToList();

        return _inventoryService.Decrease(command).IsSucceeded;
    }
}