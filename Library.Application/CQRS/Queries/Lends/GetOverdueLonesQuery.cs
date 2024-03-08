using Library.Application.ACLs;
using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.CQRS.Queries.Lends;

public record GetOverdueLonesQuery : IRequest<List<LendDto>>
{
}
internal sealed class GetOverdueLonesQueryHandler : IRequestHandler<GetOverdueLonesQuery, List<LendDto>>
{

    private readonly ILendRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IIdentityAcl _IdentityAcl;

    public GetOverdueLonesQueryHandler(ILendRepository loanRepository, IBookRepository bookRepository, IIdentityAcl identityAcl)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _IdentityAcl = identityAcl;
    }



    public async Task<List<LendDto>> Handle(GetOverdueLonesQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetAll()
            .Where(b => b.IsApproved && b.ReturnDate == null && b.IdealReturnDate < DateTime.Now).ToListAsync();
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
            Description = lend.Description ?? string.Empty,
        }).ToList();
        return result;
    }
}