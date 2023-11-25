using LibBook.Domain.BorrowAgg;
using LibBook.Domain.Services;
using LibInventory.DomainContracts.Inventory;

namespace LibBook.Infrastructure.InventoryACL;

public class LibraryInventoryAcl : ILibraryInventoryAcl
{
    private readonly IInventoryService _inventoryService;
    private readonly ILibraryIdentityAcl _IdentityAcl;

    public LibraryInventoryAcl(IInventoryService inventoryService, ILibraryIdentityAcl identityAcl)
    {
        _inventoryService = inventoryService;
        _IdentityAcl = identityAcl;
    }

    public bool LoanFromInventory(Borrow lend)
    {
        string member = _IdentityAcl.GetUserName(lend.MemberID).Result;
        var item = new DecreaseInventory(lend.BookId, 1, $"Loaned by '{member}'. " + lend.Description, lend.Id);
        if (_inventoryService.Lending(item).IsSucceeded == true)
        {
            return true;
        }
        return false;
    }
    public bool ReturnToInventory(Borrow lend)
    {
        string member = _IdentityAcl.GetUserName(lend.MemberID).Result;
        var item = new ReturnBook(lend.BookId, 1, $"Returned by '{member}'. " + lend.Description, lend.Id);
        if (_inventoryService.Returning(item).IsSucceeded == true)
        {
            return true;
        }
        return false;
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