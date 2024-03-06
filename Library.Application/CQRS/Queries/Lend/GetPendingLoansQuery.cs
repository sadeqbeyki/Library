using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record GetPendingLoansQuery : IRequest<List<LendDto>>
{
}
internal sealed class GetPendingLoansQueryHandler : IRequestHandler<GetPendingLoansQuery, List<LendDto>>
{
    private readonly ILendService _lendService;

    public GetPendingLoansQueryHandler(ILendService lendService)
    {
        _lendService = lendService;
    }

    public async Task<List<LendDto>> Handle(GetPendingLoansQuery request, CancellationToken cancellationToken)
    {
        var result = await _lendService.GetPendingLoans();
        return result;
    }
}
