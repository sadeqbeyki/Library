using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.Inventories;

namespace Warehouse.Application.CQRS.Queries.Inventories;

public record GetInventoryQuery(int id) : IRequest<EditInventory>;
internal sealed class GetInventoryQueryHandler : IRequestHandler<GetInventoryQuery, EditInventory>
{
    private readonly IInventoryRepository _inventoryRepository;

    public GetInventoryQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public Task<EditInventory> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        return  _inventoryRepository.GetDetails(request.id);

    }
}
