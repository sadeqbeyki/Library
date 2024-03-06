using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record SearchLendQuery(LendSearchModel dto) : IRequest<List<LendDto>>
{
}
internal sealed class SearchLendQueryHandler : IRequestHandler<SearchLendQuery, List<LendDto>>
{
    private readonly ILendService _lendService;

    public SearchLendQueryHandler(ILendService lendService)
    {
        _lendService = lendService;
    }

    public async Task<List<LendDto>> Handle(SearchLendQuery request, CancellationToken cancellationToken)
    {
        var result = _lendService.Search(request.dto);
        return result;
    }
}
