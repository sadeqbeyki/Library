using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Lend;

public record SearchLendQuery(LendSearchModel dto) : IRequest<List<LendDto>>
{
}
internal sealed class SearchLendQueryHandler(ILendRepository lendRepository) : IRequestHandler<SearchLendQuery, List<LendDto>>
{
    private readonly ILendRepository _lendRepository = lendRepository;

    public async Task<List<LendDto>> Handle(SearchLendQuery request, CancellationToken cancellationToken)
    {
        return _lendRepository.Search(request.dto);
    }
}
