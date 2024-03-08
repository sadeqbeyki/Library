using AppFramework.Application;
using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Lends;

public record UpdateLendCommand(LendDto dto) : IRequest<OperationResult>
{
}
internal sealed class UpdateLendCommandHandler : IRequestHandler<UpdateLendCommand, OperationResult>
{
    private readonly ILendRepository _lendRepository;
    public UpdateLendCommandHandler(ILendRepository lendRepository)
    {
        _lendRepository = lendRepository;
    }

    public async Task<OperationResult> Handle(UpdateLendCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();

        var lend = _lendRepository.GetByIdAsync(request.dto.Id).Result;
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        lend.Edit(
            request.dto.BookId,
            request.dto.MemberId,
            request.dto.EmployeeId,
            request.dto.IdealReturnDate,
            request.dto.ReturnEmployeeID,
            request.dto.ReturnDate,
            request.dto.Description);

        _lendRepository.UpdateAsync(lend);
        return operationResult.Succeeded();
    }
}
