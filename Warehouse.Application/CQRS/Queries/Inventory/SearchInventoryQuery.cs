using MediatR;
using Warehouse.Application.DTOs;
using Warehouse.Application.DTOs.Inventory;
using Warehouse.Service.Contracts;

namespace Warehouse.Application.CQRS.Queries.Inventory;

public record SearchInventoryQuery(InventorySearchModel dto) : IRequest<List<InventoryViewModel>>;
internal sealed class SearchInventoryQueryHandler : IRequestHandler<SearchInventoryQuery, List<InventoryViewModel>>
{
    private readonly IInventoryService _inventoryService;

    public SearchInventoryQueryHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<List<InventoryViewModel>> Handle(SearchInventoryQuery request, CancellationToken cancellationToken)
    {
        var result = _inventoryService.Search(request.dto);
        return result;
    }
}
