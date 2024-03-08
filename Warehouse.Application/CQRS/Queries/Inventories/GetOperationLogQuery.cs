using MediatR;
using Warehouse.Application.Contracts;
using Warehouse.Application.DTOs.InventoryOperation;

namespace Warehouse.Application.CQRS.Queries.Inventories;

public record GetOperationLogQuery(int operationId) : IRequest<List<InventoryOperationViewModel>>;
internal sealed class GetOperationLogQueryHandler : IRequestHandler<GetOperationLogQuery, List<InventoryOperationViewModel>>
{
    private readonly IInventoryRepository _inventoryRepository;

    public GetOperationLogQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<List<InventoryOperationViewModel>> Handle(GetOperationLogQuery request, CancellationToken cancellationToken)
    {
        var result =  _inventoryRepository.GetOperationLog(request.operationId);
        return result;

    }
}
