using Library.Application.ACLs;
using Library.Application.DTOs.Lends;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.CQRS.Queries.Lend;

public record GetDeletedLoansQuery : IRequest<List<LendDto>>
{
}
internal sealed class GetDeletedLoansQueryHandler : IRequestHandler<GetDeletedLoansQuery, List<LendDto>>
{

    private readonly ILendRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IIdentityAcl _IdentityAcl;

    public GetDeletedLoansQueryHandler(ILendRepository loanRepository, IBookRepository bookRepository, IIdentityAcl identityAcl)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _IdentityAcl = identityAcl;
    }


    public async Task<List<LendDto>> Handle(GetDeletedLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _loanRepository.GetAll().Where(x => x.IsDeleted).ToListAsync();
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
            ReturnEmployeeID = lend.ReturnEmployeeID,
            ReturnEmployeeName = _loanRepository.GetReturnEmployeeName(lend.ReturnEmployeeID).Result,
            //ReturnEmployeeName = _loanRepository.GetReturnEmployeeName(lend.ReturnEmployeeID).Result,
            ReturnDate = lend.ReturnDate,
            Description = lend.Description,
        }).ToList();
        return result;
    }
}