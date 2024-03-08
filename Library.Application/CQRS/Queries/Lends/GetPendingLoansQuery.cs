using Library.Application.ACLs;
using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.CQRS.Queries.Lend;

public record GetPendingLoansQuery : IRequest<List<LendDto>>
{
}
internal sealed class GetPendingLoansQueryHandler : IRequestHandler<GetPendingLoansQuery, List<LendDto>>
{
    private readonly ILendRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IIdentityAcl _IdentityAcl;

    public GetPendingLoansQueryHandler(ILendRepository loanRepository,
                                        IIdentityAcl identityAcl,
                                        IBookRepository bookRepository)
    {
        _loanRepository = loanRepository;
        _IdentityAcl = identityAcl;
        _bookRepository = bookRepository;
    }


    public async Task<List<LendDto>> Handle(GetPendingLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetAll().Where(x => !x.IsDeleted && !x.IsApproved).ToListAsync();

        var result = loans.Select(lend => new LendDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = lend.MemberID,
            MemberName = _IdentityAcl.GetUserName(lend.MemberID).Result,
            EmployeeId = lend.EmployeeId,
            EmployeeName = _IdentityAcl.GetUserName(lend.EmployeeId).Result,
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            Description = lend.Description,
        }).ToList();

        return result;
    }
}
