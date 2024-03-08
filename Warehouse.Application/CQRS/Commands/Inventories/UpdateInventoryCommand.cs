using AppFramework.Application;
using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.Inventories;

namespace Warehouse.Application.CQRS.Commands.Inventories;

public record UpdateInventoryCommand(EditInventory dto) : IRequest<OperationResult>;
internal class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, OperationResult>
{
    private readonly IInventoryRepository _inventoryRepository;

    public UpdateInventoryCommandHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<OperationResult> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        var operation = new OperationResult();
        var inventory = await _inventoryRepository.GetByIdAsync(request.dto.Id);
        if (inventory == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        if (_inventoryRepository.Exists(x => x.BookId == request.dto.BookId && x.Id != request.dto.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        inventory.Edit(request.dto.BookId, request.dto.UnitPrice);
        _inventoryRepository.SaveChanges();
        return operation.Succeeded();
    }
}

