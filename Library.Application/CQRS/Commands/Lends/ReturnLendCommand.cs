using AppFramework.Application;
using Library.Application.DTOs.Lend;
using Library.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using MediatR;
using Library.ACL.Identity;
using Library.ACL.Inventory;

namespace Library.Application.CQRS.Commands.Lends;

public record ReturnLendCommand(LendDto Dto) : IRequest<OperationResult>
{
}
internal sealed class ReturnLendCommandHandler : IRequestHandler<ReturnLendCommand, OperationResult>
{
    private readonly ILendRepository _lendRepository;
    private readonly ILibraryIdentityAcl _IdentityAcl;
    private readonly ILibraryInventoryAcl _inventoryAcl;
    public ReturnLendCommandHandler(ILendRepository lendRepository, ILibraryIdentityAcl identityAcl, ILibraryInventoryAcl inventoryAcl)
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

        if (ReturnLoan(request.Dto.Id).IsSucceeded)
        {
            await _lendRepository.UpdateAsync(lend);
            return operationResult.Succeeded();
        }
        else
        {
            return operationResult.Failed(ApplicationMessages.ReturnFailed);
        }
    }
    private OperationResult ReturnLoan(int lendId)
    {
        OperationResult operationResult = new();
        Lend lend = _lendRepository.GetByIdAsync(lendId).Result;
        if (lend == null)
            operationResult.Failed(ApplicationMessages.RecordNotFound);
        if (_inventoryAcl.ReturnToInventory(lend) == true)
        {
            lend.IsReturned = true;
            _lendRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
}
