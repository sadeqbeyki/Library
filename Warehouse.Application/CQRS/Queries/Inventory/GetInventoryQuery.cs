using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.Inventory;

namespace Warehouse.Application.CQRS.Queries.Inventory;

public record GetInventoryQuery(int id) : IRequest<EditInventory>;
internal sealed class GetInventoryQueryHandler : IRequestHandler<GetInventoryQuery, EditInventory>
{
    private readonly IInventoryService _inventoryService;

    public GetInventoryQueryHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public Task<EditInventory> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        var result = _inventoryService.GetDetails(request.id);
        return Task.FromResult(result);
    }
}
