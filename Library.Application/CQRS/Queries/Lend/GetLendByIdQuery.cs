using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record GetLendByIdQuery(int id) : IRequest<LendDto>
{
}

internal sealed class GetLendByIdQueryHandler : IRequestHandler<GetLendByIdQuery, LendDto>
{
    private readonly ILendService _lendService;
    public async Task<LendDto> Handle(GetLendByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _lendService.GetLendById(request.id);
        return result;
    }
}
