using AppFramework.Application;
using Library.Application.ACLs;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.InventoryOperation;

namespace Warehouse.Application.CQRS.Commands.Inventories;

public record IncreaseInventoryItemCommand(IncreaseInventory dto) : IRequest<OperationResult>;

public class IncreaseInventoryItemCommandHandler : IRequestHandler<IncreaseInventoryItemCommand, OperationResult>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IIdentityAcl _identityAcl;
    public IncreaseInventoryItemCommandHandler(IInventoryRepository inventoryRepository, IIdentityAcl identityAcl)
    {
        _inventoryRepository = inventoryRepository;
        _identityAcl = identityAcl;
    }

    public async Task<OperationResult> Handle(IncreaseInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var operationResult = new OperationResult();
        var inventory = await _inventoryRepository.GetByIdAsync(request.dto.InventoryId);
        if (inventory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        var operatorId = _identityAcl.GetCurrentUserId();
        inventory.Increase(request.dto.Count, operatorId, request.dto.Description);
        _inventoryRepository.SaveChanges();
        return operationResult.Succeeded();
    }
}