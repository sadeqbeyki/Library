using Identity.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs;

namespace Library.ACL.Inventory;

public class LibraryInventoryAcl : ILibraryInventoryAcl
{
    private readonly IInventoryService _inventoryService;
    private readonly IUserService _userService;

    public LibraryInventoryAcl(IInventoryService inventoryService, IUserService userService)
    {
        _inventoryService = inventoryService;
        _userService = userService;
    }

    public bool BorrowFromInventory(Lend lend)
    {
        string member = _userService.GetUserNameAsync(lend.MemberID).Result;
        var item = new DecreaseInventory(lend.BookId, 1, $"Borrowed by '{member}'. " + lend.Description, lend.Id);
        if (_inventoryService.Lending(item).IsSucceeded == true)
        {
            return true;
        }
        return false;
    }
    public bool ReturnToInventory(Lend lend)
    {
        string member = _userService.GetUserNameAsync(lend.MemberID).Result;
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