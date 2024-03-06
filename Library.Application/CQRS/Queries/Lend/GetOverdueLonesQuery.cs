using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record GetOverdueLonesQuery : IRequest<List<LendDto>>
{
}
internal sealed class GetOverdueLonesQueryHandler : IRequestHandler<GetOverdueLonesQuery, List<LendDto>>
{
    private readonly ILendService _lendService;

    public GetOverdueLonesQueryHandler(ILendService lendService)
    {
        _lendService = lendService;
    }

    public async Task<List<LendDto>> Handle(GetOverdueLonesQuery request, CancellationToken cancellationToken)
    {
        var result = await _lendService.GetOverdueLones();
        return result;
    }
}