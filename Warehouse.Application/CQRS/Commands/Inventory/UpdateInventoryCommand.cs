using AppFramework.Application;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.Inventory;

namespace Warehouse.Application.CQRS.Commands.Inventory;

public record UpdateInventoryCommand(EditInventory dto) :IRequest<OperationResult>;
internal class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, OperationResult>
{
    private readonly IInventoryService _inventoryService;

    public UpdateInventoryCommandHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public Task<OperationResult> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        var result = _inventoryService.Edit(request.dto);
        return result;
    }
}

