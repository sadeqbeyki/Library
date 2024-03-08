using AppFramework.Application;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.Inventories;
using Warehouse.Domain.Entities.InventoryAgg;

namespace Warehouse.Application.CQRS.Commands.Inventories;

public record CreateInventoryCommand(CreateInventory dto) : IRequest<OperationResult>;

public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, OperationResult>
{
    private readonly IInventoryRepository _inventoryRepository;

    public CreateInventoryCommandHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<OperationResult> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        OperationResult operation = new();

        var isExist = _inventoryRepository.Exists(x => x.BookId == request.dto.BookId);
        if (isExist)
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var inventory = new Inventory(request.dto.BookId, request.dto.UnitPrice);
        await _inventoryRepository.CreateAsync(inventory);
        return operation.Succeeded();
    }
}