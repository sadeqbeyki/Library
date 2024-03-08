using Library.Application.ACLs;
using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;

using MediatR;

namespace Library.Application.CQRS.Queries.Lends;

public record GetLendByIdQuery(int lendId) : IRequest<LendDto>
{
}

internal sealed class GetLendByIdQueryHandler(ILendRepository lendRepository,
                                              IBookRepository bookRepository,
                                              IIdentityAcl identityAcl) : IRequestHandler<GetLendByIdQuery, LendDto>
{
    private readonly ILendRepository _lendRepository = lendRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IIdentityAcl _identityAcl = identityAcl;
    public async Task<LendDto> Handle(GetLendByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.LendAgg.Lend lend = await _lendRepository.GetByIdAsync(request.lendId);
        var book = await _bookRepository.GetByIdAsync(lend.BookId);

        LendDto dto = new()
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = book.Title,
            MemberId = lend.MemberID,
            MemberName = await _identityAcl.GetUserName(lend.MemberID),
            EmployeeId = lend.EmployeeId,
            EmployeeName = await _identityAcl.GetUserName(lend.EmployeeId),
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            ReturnDate = lend.ReturnDate,
            ReturnEmployeeID = lend.ReturnEmployeeID,
            ReturnEmployeeName = await _identityAcl.GetUserName(lend.ReturnEmployeeID),
            //ReturnEmployeeName = _loanRepository.GetReturnEmployeeName(lend.ReturnEmployeeID).Result,
            Description = lend.Description,
        };
        return dto;
    }
}
