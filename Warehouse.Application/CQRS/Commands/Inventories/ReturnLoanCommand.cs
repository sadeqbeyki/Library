using AppFramework.Application;
using Library.Application.ACLs;
using Library.Domain.Entities.LendAgg;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.InventoryOperation;
using Warehouse.Domain.Entities.InventoryAgg;

namespace Warehouse.Application.CQRS.Commands.Inventories;

public record ReturnLoanCommand(Lend dto) : IRequest<bool>;

public class ReturnLoanCommandHandler : IRequestHandler<ReturnLoanCommand, bool>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IIdentityAcl _identityAcl;
    public ReturnLoanCommandHandler(IInventoryRepository inventoryRepository, IIdentityAcl identityAcl)
    {
        _inventoryRepository = inventoryRepository;
        _identityAcl = identityAcl;
    }

    public async Task<bool> Handle(ReturnLoanCommand request, CancellationToken cancellationToken)
    {
        string member = _identityAcl.GetUserName(request.dto.MemberID).Result;
        var item = new ReturnBook(request.dto.BookId, 1, $"Returned by '{member}'. " + request.dto.Description, request.dto.Id);
        if (Returning(item).IsSucceeded == true)
        {
            return true;
        }
        return false;
    }

    private OperationResult Returning(ReturnBook command)
    {
        OperationResult operationResult = new();

        var inventory = _inventoryRepository.GetBy(command.BookId);
        if (inventory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        //if (inventory.IsLoaned == false)
        //    return operationResult.Failed(ApplicationMessages.BookWasAlreadyReturned);

        var returnResult = ReturnToInventory(inventory, command.Count, command.Description, command.LendId);
        if (returnResult.IsSucceeded)
        {
            _inventoryRepository.SaveChanges();
        }
        else
        {
            return operationResult.Failed(ApplicationMessages.ReturnFailed);
        }
        return operationResult.Succeeded();
    }
    private OperationResult ReturnToInventory(Inventory inventory, long count, string description, int lendId)
    {
        OperationResult operation = new();
        try
        {
            inventory.IsLoaned = false;
            inventory.Return(count, _identityAcl.GetCurrentUserId(), description, lendId);
            return operation.Succeeded();
        }
        catch
        {
            return operation.Failed(ApplicationMessages.ReturnFailed);
        }
    }
}