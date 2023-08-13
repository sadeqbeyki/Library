using LibBook.Domain.BorrowAgg;
using LibBook.Domain.Services;
using LibInventory.DomainContracts.Inventory;

namespace LibBook.Infrastructure.InventoryACL;

public class LibraryInventoryAcl : ILibraryInventoryAcl
{
    private readonly IInventoryService _inventoryService;

    public LibraryInventoryAcl(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public bool BorrowFromInventory(Borrow borrow)
    {
        var item = new DecreaseInventory(borrow.BookId, 1, "Borrowed...", borrow.Id);

        return _inventoryService.Borrowing(item).IsSucceeded;
    }
    public bool ReturnToInventory(Borrow borrow)
    {
        var item = new ReturnBook(borrow.BookId, 1, "Returned...", borrow.Id);
        return _inventoryService.Returning(item).IsSucceeded;
    }

    //public bool LendFromInventory(List<LendItem> items)
    //{
    //    var command = items.Select(l =>
    //        new DecreaseInventory(
    //                        l.BookId,
    //                        1,
    //                        "In Lend...",
    //                        l.LendId
    //                )).ToList();

    //    return _inventoryService.Decrease(command).IsSucceeded;
    //}


}