using Library.Application.ACLs;
using Library.Domain.Entities.LendAgg;
using MediatR;
using Warehouse.Application.CQRS.Commands.Inventories;

namespace Inventory.ACL.Inventory;

public class LibraryInventoryAcl : ILibraryInventoryAcl
{
    private readonly IMediator _mediator;

    public LibraryInventoryAcl(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<bool> BorrowFromInventory(Lend lend)
    {
        var result = await _mediator.Send(new BorrowingCommand(lend));
        if (result)
            return true;
        return false;
    }
    public async Task<bool> ReturnToInventory(Lend lend)
    {
        bool result =await _mediator.Send(new ReturnLoanCommand(lend));
        if (result)
            return true;
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