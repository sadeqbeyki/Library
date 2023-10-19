﻿using LibBook.Domain.BorrowAgg;
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

    public bool LoanFromInventory(Borrow lend)
    {
        var item = new DecreaseInventory(lend.BookId, 1, "Borrowed...", lend.Id);
        if (_inventoryService.Borrowing(item).IsSucceeded == true)
        {
            return true;
        }
        return false;
    }
    public bool ReturnToInventory(Borrow borrow)
    {
        var item = new ReturnBook(borrow.BookId, 1, "Returned...", borrow.Id);
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