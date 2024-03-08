using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;

using MediatR;

namespace Library.Application.CQRS.Queries.Lends;

public record GetLoansByMemberIdQuery(Guid memberId) : IRequest<List<LendDto>>
{
}

internal sealed class GetLoansByMemberIdQueryHandler(ILendRepository lendRepository) : IRequestHandler<GetLoansByMemberIdQuery, List<LendDto>>
{
    private readonly ILendRepository _lendRepository = lendRepository;

    public async Task<List<LendDto>> Handle(GetLoansByMemberIdQuery request, CancellationToken cancellationToken)
    {
        return await _lendRepository.GetLoansByMemberId(request.memberId);
    }
}
