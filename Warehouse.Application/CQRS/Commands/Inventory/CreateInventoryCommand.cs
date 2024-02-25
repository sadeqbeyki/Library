using AppFramework.Application;
using MediatR;
using Warehouse.Application.DTOs.Inventory;
using Warehouse.Service.Contracts;

namespace Warehouse.Application.CQRS.Commands.Inventory;

public record CreateInventoryCommand(CreateInventory dto) : IRequest<OperationResult>;

public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, OperationResult>
{
    private readonly IInventoryService _inventoryService;

    public CreateInventoryCommandHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<OperationResult> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _inventoryService.Create(request.dto);
        return result;
    }
}