using BI.ApplicationContracts.Inventory;
using LMS.Domain.BorrowAgg;
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

    public bool BorrowFromInventory(Borrow item)
    {
        var command = new DecreaseInventory(item.BookId, 1, "Borrow...", item.Id);

        return _inventoryService.Borrowing(command).IsSucceeded;
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