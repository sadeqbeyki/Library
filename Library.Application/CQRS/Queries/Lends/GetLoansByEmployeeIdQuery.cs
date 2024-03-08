using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;

using MediatR;

namespace Library.Application.CQRS.Queries.Lends;

public record GetLoansByEmployeeIdQuery(Guid employeeId) : IRequest<List<LendDto>>
{
}

internal sealed class GetLoansByEmployeeIdQueryHandler(ILendRepository lendRepository) : IRequestHandler<GetLoansByEmployeeIdQuery, List<LendDto>>
{
    private readonly ILendRepository _lendRepository = lendRepository;

    public async Task<List<LendDto>> Handle(GetLoansByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        return await _lendRepository.GetLoansByEmployeeId(request.employeeId);
    }
}
