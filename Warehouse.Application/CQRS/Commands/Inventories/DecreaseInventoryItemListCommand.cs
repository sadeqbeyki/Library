using AppFramework.Application;
using Library.Application.ACLs;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.InventoryOperation;

namespace Warehouse.Application.CQRS.Commands.Inventories;

public record DecreaseInventoryItemCommand(DecreaseInventory dto) : IRequest<OperationResult>;

public class DecreaseInventoryItemCommandHandler : IRequestHandler<DecreaseInventoryItemCommand, OperationResult>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IIdentityAcl _identityAcl;
    public DecreaseInventoryItemCommandHandler(IInventoryRepository inventoryRepository, IIdentityAcl identityAcl)
    {
        _inventoryRepository = inventoryRepository;
        _identityAcl = identityAcl;
    }

    public async Task<OperationResult> Handle(DecreaseInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();

        var inventory = await _inventoryRepository.GetByIdAsync(request.dto.InventoryId);
        if (inventory == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        try
        {
            var operatorId = _identityAcl.GetCurrentUserId();
            inventory.Decrease(request.dto.Count, operatorId, request.dto.Description, request.dto.LendId);

            _inventoryRepository.SaveChanges();
            return operation.Succeeded();
        }
        catch (InvalidOperationException ex)
        {
            return operation.Failed(ex.Message);
        }
    }
}