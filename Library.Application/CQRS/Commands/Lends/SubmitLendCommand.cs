using AppFramework.Application;
using Library.Application.ACLs;
using Library.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using MediatR;

namespace Library.Application.CQRS.Commands.Lends;

public record SubmitLendCommand(int lendId) : IRequest<OperationResult>
{
}

internal sealed class SubmitLendCommandHandler : IRequestHandler<SubmitLendCommand, OperationResult>
{
    private readonly ILendRepository _lendRepository;
    private readonly ILibraryInventoryAcl _inventoryAcl;

    public SubmitLendCommandHandler(ILendRepository loanRepository,
                                    ILibraryInventoryAcl inventoryAcl)
    {
        _lendRepository = loanRepository;
        _inventoryAcl = inventoryAcl;
    }

    public async Task<OperationResult> Handle(SubmitLendCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();

        Lend lend = await _lendRepository.GetByIdAsync(request.lendId);
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        //lend duplicate book check
        var memberDuplicateLoans = await _lendRepository.GetDuplicatedLoans(lend.MemberID, lend.BookId);
        if (memberDuplicateLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.DuplicateLendByMember);

        //Member Overdue Loans
        var memberOverdueLoans = await _lendRepository.GetMemberOverdueLoans(lend.MemberID);
        if (memberOverdueLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.MemberDidntReturnedTheBook);

        if (await _inventoryAcl.BorrowFromInventory(lend) == true)
        {
            lend.IsApproved = true;
            _lendRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
}
