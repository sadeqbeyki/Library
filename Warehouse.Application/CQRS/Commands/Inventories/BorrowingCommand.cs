using AppFramework.Application;
using Library.Application.ACLs;
using Library.Domain.Entities.LendAgg;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.InventoryOperation;
using Warehouse.Domain.Entities.InventoryAgg;

namespace Warehouse.Application.CQRS.Commands.Inventories;

public record BorrowingCommand(Lend dto) : IRequest<bool>;

internal sealed class BorrowingCommandHandler : IRequestHandler<BorrowingCommand, bool>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IIdentityAcl _identityAcl;
    public BorrowingCommandHandler(IInventoryRepository inventoryRepository, IIdentityAcl identityAcl)
    {
        _inventoryRepository = inventoryRepository;
        _identityAcl = identityAcl;
    }

    public async Task<bool> Handle(BorrowingCommand request, CancellationToken cancellationToken)
    {
        string member = _identityAcl.GetUserName(request.dto.MemberID).Result;
        var item = new DecreaseInventory(request.dto.BookId, 1, $"Borrowed by '{member}'. " + request.dto.Description, request.dto.Id);
        if (Lending(item).IsSucceeded == true)
        {
            return true;
        }
        return false;
    }

    private OperationResult Lending(DecreaseInventory command)
    {
        OperationResult operationResult = new();

        var inventory = _inventoryRepository.GetBy(command.BookId);
        if (inventory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        if (inventory.IsLoaned == true && inventory.CalculateCurrentCount() <= 0)
            return operationResult.Failed(ApplicationMessages.BookIsLoaned);

        var lendResult = LoanFromInventory(inventory, command.Count, command.Description, command.LendId);

        if (lendResult.IsSucceeded)
        {
            _inventoryRepository.SaveChanges();
        }
        else
        {
            return lendResult.Failed(ApplicationMessages.LendFailed);
        }

        return operationResult.Succeeded();
    }
    private OperationResult LoanFromInventory(Inventory inventory, long count, string description, int lendId)
    {
        OperationResult operation = new();
        try
        {
            inventory.IsLoaned = true;
            inventory.Decrease(count, _identityAcl.GetCurrentUserId(), description, lendId);
            return operation.Succeeded();
        }
        catch /*(InvalidOperationException ex)*/
        {
            return operation.Failed(ApplicationMessages.TheBookIsNotInStock);
        }
    }
}