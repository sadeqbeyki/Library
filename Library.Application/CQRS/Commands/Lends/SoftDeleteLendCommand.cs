using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using MediatR;

namespace Library.Application.CQRS.Commands.Lends;

public record SoftDeleteLendCommand(LendDto Dto) : IRequest
{
}
internal sealed class SoftDeleteLendCommandHandler : IRequestHandler<SoftDeleteLendCommand>
{
    private readonly ILendRepository _lendRepository;
    public SoftDeleteLendCommandHandler(ILendRepository lendRepository)
    {
        _lendRepository = lendRepository;
    }

    public async Task Handle(SoftDeleteLendCommand request, CancellationToken cancellationToken)
    {
        Lend lend = _lendRepository.GetByIdAsync(request.Dto.Id).Result;
        _lendRepository.SoftDelete(lend);
    }
}