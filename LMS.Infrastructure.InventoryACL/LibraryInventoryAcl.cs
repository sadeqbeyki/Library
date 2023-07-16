using BI.ApplicationContracts.Inventory;
using LendBook.Domain.LendAgg;
using LendBook.Domain.RentAgg;
using LendBook.Domain.Services;

namespace LendBook.Infrastructure.InventoryACL;

public class LibraryInventoryAcl : ILibraryInventoryAcl
{
    private readonly IInventoryService _inventoryService;

    public LibraryInventoryAcl(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public bool DecreaseFromInventory(List<RentItem> items)
    {
        var command = items.Select(rentItem =>
                new DecreaseInventory(rentItem.BookId, rentItem.Count, "امانت عضو", rentItem.RentId))
            .ToList();

        return _inventoryService.Decrease(command).IsSucceeded;
    }

    public bool LendFromInventory(List<LendItem> items)
    {
        var command = items.Select(l =>
            new DecreaseInventory(
                            l.BookId,
                            l.Count,
                            "Member Lend",
                            l.LendId
                    )).ToList();

        return _inventoryService.Decrease(command).IsSucceeded;
    }
}