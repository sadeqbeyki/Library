using AppFramework.Domain;
using LibBook.Domain.BookAgg;
using LibBook.Domain.BorrowAgg;
using LibBook.Domain.Services;
using LibBook.DomainContracts.Borrow;
using Microsoft.EntityFrameworkCore;


namespace LibBook.Infrastructure.Repositories;

public partial class LoanRepository : Repository<Borrow, int>, ILoanRepository
{
    private readonly BookDbContext _bookDbContext;
    private readonly ILibraryIdentityAcl _IdentityAcl;
    private readonly IBookRepository _bookRepository;
    public LoanRepository(BookDbContext dbContext, ILibraryIdentityAcl identityAcl, IBookRepository bookRepository) : base(dbContext)
    {
        _bookDbContext = dbContext;
        _IdentityAcl = identityAcl;
        _bookRepository = bookRepository;
    }

    public async Task<List<LoanDto>> GetBorrowsByEmployeeId(Guid employeeId)
    {
        var borrows = await _bookDbContext.Borrows.Where(x => x.EmployeeId == employeeId).ToListAsync();
        List<LoanDto> result = borrows.Select(b => new LoanDto
        {
            Id = b.Id,
            BookId = b.BookId,
            BookTitle = _bookRepository.GetByIdAsync(b.BookId).Result.Title,
            MemberId = b.MemberID,
            MemberName = _IdentityAcl.GetUserName(b.MemberID).Result,
            EmployeeId = b.EmployeeId,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeId = Guid.Parse(_IdentityAcl.GetUserName(b.ReturnEmployeeID).Result),
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetBorrowsByMemberId(Guid memberId)
    {
        var loans = await _bookDbContext.Borrows
            .Where(x => x.MemberID == memberId)
            .ToListAsync();
        List<LoanDto> result = loans.Select(b => new LoanDto
        {
            Id = b.Id,
            BookId = b.BookId,
            BookTitle = _bookRepository.GetByIdAsync(b.BookId).Result.Title,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            EmployeeName = _IdentityAcl.GetUserName(b.EmployeeId).Result,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeId = Guid.Parse(_IdentityAcl.GetUserName(b.ReturnEmployeeID).Result),
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetDuplicatedLoans(Guid memberId, int bookId)
    {
        var loans = await _bookDbContext.Borrows
            .Where(b => b.MemberID == memberId && b.BookId == bookId && b.IsApproved && !b.IsReturned && !b.IsDeleted).ToListAsync();

        List<LoanDto> result = loans.Select(b => new LoanDto
        {
            Id = b.Id,
            BookId = b.BookId,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeId = b.ReturnEmployeeID,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetMemberOverdueLoans(Guid memberId)
    {
        var loans = await _bookDbContext.Borrows
            .Where(b => b.MemberID == memberId && b.IsApproved && b.IdealReturnDate < DateTime.Now && !b.IsReturned && !b.IsDeleted).ToListAsync();

        List<LoanDto> result = loans.Select(b => new LoanDto
        {
            Id = b.Id,
            BookId = b.BookId,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeId = b.ReturnEmployeeID,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public List<LoanDto> Search(LoanSearchModel searchModel)
    {
        var borrows = _bookDbContext.Borrows.Where(x => !x.IsDeleted).ToList();
        var query = borrows
        .Select(x => new LoanDto
        {
            Id = x.Id,
            BookId = x.BookId,
            BookTitle = _bookRepository.GetByIdAsync(x.BookId).Result.Title,
            MemberId = x.MemberID,
            MemberName = _IdentityAcl.GetUserName(x.MemberID).Result,
            EmployeeId = x.EmployeeId,
            EmployeeName = _IdentityAcl.GetUserName(x.EmployeeId).Result,
            CreationDate = x.CreationDate,
            IdealReturnDate = x.IdealReturnDate,
            ReturnEmployeeId = x.ReturnEmployeeID,
            ReturnEmployeeName = _IdentityAcl.GetUserName(x.ReturnEmployeeID).Result,
            ReturnDate = x.ReturnDate,
        });

        if (!string.IsNullOrWhiteSpace(searchModel.MemberName))
            query = query.Where(x => x.MemberName.Contains(searchModel.MemberName));

        if (!string.IsNullOrEmpty(searchModel.EmployeeName))
            query = query.Where(x => x.EmployeeName.Contains(searchModel.EmployeeName));

        if (searchModel.BookId != 0)
            query = query.Where(x => x.BookId == searchModel.BookId);

        return query.OrderByDescending(x => x.Id).ToList();
    }
}
