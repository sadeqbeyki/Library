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

    public async Task<List<LoanDto>> GetBorrowsByEmployeeId(string employeeId)
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
            ReturnEmployeeId = _IdentityAcl.GetUserName(b.ReturnEmployeeID).Result,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetBorrowsByMemberId(string memberId)
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
            ReturnEmployeeId = _IdentityAcl.GetUserName(b.ReturnEmployeeID).Result,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetDuplicatedLoans(string memberId, int bookId)
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

    public async Task<List<LoanDto>> GetMemberOverdueLoans(string memberId)
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

    public async Task<List<LoanDto>> Search(LoanSearchModel searchModel)
    {
        string bookTitle =  _bookRepository.GetByIdAsync(searchModel.BookId).Result.Title;
        var query = _bookDbContext.Borrows
        .Select(x => new LoanDto
        {
            Id = x.Id,
            BookId = x.BookId,
            BookTitle = bookTitle,
        });

        if (!string.IsNullOrWhiteSpace(searchModel.BookTitle))
            query = query.Where(x => x.BookTitle.Contains(searchModel.BookTitle));

        return query.OrderByDescending(x => x.Id).ToList();
    }

    //public async Task<List<LoanDto>> GetOverdueLones()
    //{
    //    var loans = await _bookDbContext.Borrows
    //        .Where(b => b.ReturnDate == null && b.IdealReturnDate < DateTime.Now).ToListAsync();

    //    List<LoanDto> result = loans.Select(b => new LoanDto
    //    {
    //        Id = b.Id,
    //        BookId = b.BookId,
    //        BookTitle = _bookDbContext.Books.Find(b.BookId).Title ?? string.Empty,
    //        MemberId = b.MemberID,
    //        EmployeeId = b.EmployeeId,
    //        CreationDate = b.CreationDate,
    //        IdealReturnDate = b.IdealReturnDate,
    //        ReturnEmployeeId = b.ReturnEmployeeID,
    //        ReturnDate = b.ReturnDate,
    //        Description = b.Description
    //    }).ToList();
    //    return result;
    //}
}
