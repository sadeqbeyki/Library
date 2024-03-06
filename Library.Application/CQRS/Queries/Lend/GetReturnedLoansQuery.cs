using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record GetReturnedLoansQuery : IRequest<List<LendDto>>
{
}

internal sealed class GetReturnedLoansQueryHandler : IRequestHandler<GetReturnedLoansQuery, List<LendDto>>
{
    private readonly ILendService _lendService;

    public GetReturnedLoansQueryHandler(ILendService lendService)
    {
        _lendService = lendService;
    }

    public async Task<List<LendDto>> Handle(GetReturnedLoansQuery request, CancellationToken cancellationToken)
    {
        var result = await _lendService.GetReturnedLoans();
        return result;
    }
}