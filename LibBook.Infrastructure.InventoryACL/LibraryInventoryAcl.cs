using Identity.Application.Interfaces;
using LibBook.Domain.BorrowAgg;
using LibBook.Domain.Services;
using Warehouse.Application.DTOs;
using Warehouse.Service.Contracts;

namespace LibBook.Infrastructure.InventoryACL;

public class LibraryInventoryAcl : ILibraryInventoryAcl
{
    private readonly IInventoryService _inventoryService;
    private readonly IUserService _userService;

    public LibraryInventoryAcl(IInventoryService inventoryService, IUserService userService)
    {
        _inventoryService = inventoryService;
        _userService = userService;
    }

    public bool LoanFromInventory(Borrow lend)
    {
        string member = _userService.GetUserNameAsync(lend.MemberID).Result;
        var item = new DecreaseInventory(lend.BookId, 1, $"Borrowed by '{member}'. " + lend.Description, lend.Id);
        if (_inventoryService.Lending(item).IsSucceeded == true)
        {
            return true;
        }
        return false;
    }
    public bool ReturnToInventory(Borrow lend)
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