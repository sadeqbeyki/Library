using AppFramework.Application;
using Library.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using MediatR;
using Library.Application.DTOs.Lends;
using Library.Application.ACLs;

namespace Library.Application.CQRS.Commands.Lends;

public record ReturnLendCommand(LendDto Dto) : IRequest<OperationResult>
{
}
internal sealed class ReturnLendCommandHandler : IRequestHandler<ReturnLendCommand, OperationResult>
{
    private readonly ILendRepository _lendRepository;
    private readonly IIdentityAcl _IdentityAcl;
    private readonly ILibraryInventoryAcl _inventoryAcl;
    public ReturnLendCommandHandler(ILendRepository lendRepository, IIdentityAcl identityAcl, ILibraryInventoryAcl inventoryAcl)
    {
        _lendRepository = lendRepository;
        _IdentityAcl = identityAcl;
        _inventoryAcl = inventoryAcl;
    }

    public async Task<OperationResult> Handle(ReturnLendCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();

        var lend = _lendRepository.GetByIdAsync(request.Dto.Id).Result;
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);
        var returnEmployeeID = _IdentityAcl.GetCurrentUserId();
        lend.Edit(request.Dto.BookId, request.Dto.MemberId, request.Dto.EmployeeId,
                    request.Dto.IdealReturnDate, returnEmployeeID, DateTime.Now, request.Dto.Description);

        var result = await ReturnLoan(request.Dto.Id);
        if (result.IsSucceeded)
        {
            await _lendRepository.UpdateAsync(lend);
            return operationResult.Succeeded();
        }
        else
        {
            return operationResult.Failed(ApplicationMessages.ReturnFailed);
        }
    }
    private async Task<OperationResult> ReturnLoan(int lendId)
    {
        OperationResult operationResult = new();
        Lend lend = _lendRepository.GetByIdAsync(lendId).Result;
        if (lend == null)
            operationResult.Failed(ApplicationMessages.RecordNotFound);
        var result = await _inventoryAcl.ReturnToInventory(lend);
        if (result)
        {
            lend.IsReturned = true;
            _lendRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
}
