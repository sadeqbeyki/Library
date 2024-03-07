using AppFramework.Application;
using Library.Application.DTOs.Lend;
using Library.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using MediatR;
using Library.ACL.Identity;

namespace Library.Application.CQRS.Commands.Lends;

public record LendingCommand(LendDto Dto) : IRequest<OperationResult>
{
}
internal sealed class LendingCommandHandler : IRequestHandler<LendingCommand, OperationResult>
{
    private readonly ILendRepository _lendRepository;
    private readonly ILibraryIdentityAcl _IdentityAcl;
    public LendingCommandHandler(ILendRepository lendRepository, ILibraryIdentityAcl identityAcl)
    {
        _lendRepository = lendRepository;
        _IdentityAcl = identityAcl;
    }

    public async Task<OperationResult> Handle(LendingCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();
        if (request.Dto.BookId <= 0 || request.Dto.MemberId == null)
            return operationResult.Failed(ApplicationMessages.ModelIsNull);

        Lend lend = new(
            request.Dto.BookId,
            request.Dto.MemberId,
            _IdentityAcl.GetCurrentUserId(),
            request.Dto.IdealReturnDate,
            request.Dto.ReturnEmployeeID,
            request.Dto.ReturnDate,
            request.Dto.Description
            );

        var result = await _lendRepository.CreateAsync(lend);
        return operationResult.Succeeded();
    }
}
