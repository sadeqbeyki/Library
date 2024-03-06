using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record GetApprovedLoansQuery : IRequest<List<LendDto>>
{
}
internal sealed class GetApprovedLoansQueryHandler : IRequestHandler<GetApprovedLoansQuery, List<LendDto>>
{
    private readonly ILendService _lendService;

    public GetApprovedLoansQueryHandler(ILendService lendService)
    {
        _lendService = lendService;
    }

    public async Task<List<LendDto>> Handle(GetApprovedLoansQuery request, CancellationToken cancellationToken)
    {
        var result = await _lendService.GetApprovedLoans();
        return result;
    }
}
