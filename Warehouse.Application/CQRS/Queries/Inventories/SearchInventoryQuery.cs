using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs;
using Warehouse.Application.DTOs.Inventories;

namespace Warehouse.Application.CQRS.Queries.Inventories;

public record SearchInventoryQuery(InventorySearchModel dto) : IRequest<List<InventoryViewModel>>;
internal sealed class SearchInventoryQueryHandler : IRequestHandler<SearchInventoryQuery, List<InventoryViewModel>>
{
    private readonly IInventoryRepository _inventoryRepository;

    public SearchInventoryQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<List<InventoryViewModel>> Handle(SearchInventoryQuery request, CancellationToken cancellationToken)
    {
        var result = _inventoryRepository.Search(request.dto);
        return result;
    }
}
