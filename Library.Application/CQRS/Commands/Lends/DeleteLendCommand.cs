using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Lends;

public record DeleteLendCommand(int lendId) : IRequest
{
}
internal sealed class DeleteLendCommandHandler : IRequestHandler<DeleteLendCommand>
{
    private readonly ILendRepository _lendRepository;
    public DeleteLendCommandHandler(ILendRepository lendRepository)
    {
        _lendRepository = lendRepository;
    }

    public async Task Handle(DeleteLendCommand request, CancellationToken cancellationToken)
    {
        var lend = await _lendRepository.GetByIdAsync(request.lendId);
        await _lendRepository.DeleteAsync(lend);
    }
}
