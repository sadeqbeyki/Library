using AppFramework.Application;
using Library.Application.ACLs;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.InventoryOperation;

namespace Warehouse.Application.CQRS.Commands.Inventories;

public record DecreaseInventoryItemListCommand(List<DecreaseInventory> decreaseList) : IRequest<OperationResult>;

public class DecreaseInventoryItemListCommandHandler : IRequestHandler<DecreaseInventoryItemListCommand, OperationResult>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IIdentityAcl _identityAcl;
    public DecreaseInventoryItemListCommandHandler(IInventoryRepository inventoryRepository, IIdentityAcl identityAcl)
    {
        _inventoryRepository = inventoryRepository;
        _identityAcl = identityAcl;
    }

    public async Task<OperationResult> Handle(DecreaseInventoryItemListCommand request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var operatorId = _identityAcl.GetCurrentUserId();
        foreach (var item in request.decreaseList)
        {
            var inventory = _inventoryRepository.GetBy(item.BookId);
            inventory.Decrease(item.Count, operatorId, item.Description, item.LendId);
        }
        _inventoryRepository.SaveChanges();
        return operation.Succeeded();
    }
}