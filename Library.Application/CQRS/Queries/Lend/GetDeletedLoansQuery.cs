using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record GetDeletedLoansQuery : IRequest<List<LendDto>>
{
}
internal sealed class GetDeletedLoansQueryHandler : IRequestHandler<GetDeletedLoansQuery, List<LendDto>>
{
    private readonly ILendService _lendService;

    public GetDeletedLoansQueryHandler(ILendService lendService)
    {
        _lendService = lendService;
    }

    public async Task<List<LendDto>> Handle(GetDeletedLoansQuery request, CancellationToken cancellationToken)
    {
        var result = await _lendService.GetDeletedLoans();
        return result;
    }
}