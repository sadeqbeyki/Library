using AppFramework.Domain;
using Library.ACL.Identity;
using Library.Application.DTOs.Lend;
using Library.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using Microsoft.EntityFrameworkCore;


namespace Library.Persistance.Repositories;

public partial class LendRepository : Repository<Lend, int>, ILendRepository
{
    private readonly BookDbContext _bookDbContext;
    private readonly ILibraryIdentityAcl _IdentityAcl;
    private readonly IBookRepository _bookRepository;
    public LendRepository(BookDbContext dbContext, ILibraryIdentityAcl identityAcl, IBookRepository bookRepository) : base(dbContext)
    {
        _bookDbContext = dbContext;
        _IdentityAcl = identityAcl;
        _bookRepository = bookRepository;
    }

    public async Task<List<LendDto>> GetLoansByEmployeeId(Guid employeeId)
    {
        var loans = await _bookDbContext.Loans.Where(x => x.EmployeeId == employeeId).ToListAsync();
        List<LendDto> result = loans.Select(b => new LendDto
        {
            Id = b.Id,
            BookId = b.BookId,
            BookTitle = _bookRepository.GetByIdAsync(b.BookId).Result.Title,
            MemberId = b.MemberID,
            MemberName = _IdentityAcl.GetUserName(b.MemberID).Result,
            EmployeeId = b.EmployeeId,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeName = GetReturnEmployeeName(b.ReturnEmployeeID).Result,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LendDto>> GetLoansByMemberId(Guid memberId)
    {
        var loans = await _bookDbContext.Loans
            .Where(x => x.MemberID == memberId)
            .ToListAsync();
        List<LendDto> result = loans.Select(b => new LendDto
        {
            Id = b.Id,
            BookId = b.BookId,
            BookTitle = _bookRepository.GetByIdAsync(b.BookId).Result.Title,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            EmployeeName = _IdentityAcl.GetUserName(b.EmployeeId).Result,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeName = GetReturnEmployeeName(b.ReturnEmployeeID).Result,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LendDto>> GetDuplicatedLoans(Guid memberId, int bookId)
    {
        var loans = await _bookDbContext.Loans
            .Where(b => b.MemberID == memberId && b.BookId == bookId && b.IsApproved && !b.IsReturned && !b.IsDeleted).ToListAsync();

        List<LendDto> result = loans.Select(b => new LendDto
        {
            Id = b.Id,
            BookId = b.BookId,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeID = b.ReturnEmployeeID,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LendDto>> GetMemberOverdueLoans(Guid memberId)
    {
        var loans = await _bookDbContext.Loans
            .Where(b => b.MemberID == memberId && b.IsApproved && b.IdealReturnDate < DateTime.Now && !b.IsReturned && !b.IsDeleted).ToListAsync();

        List<LendDto> result = loans.Select(b => new LendDto
        {
            Id = b.Id,
            BookId = b.BookId,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            CreationDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeID = b.ReturnEmployeeID,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public List<LendDto> Search(LendSearchModel searchModel)
    {
        var loans = _bookDbContext.Loans.Where(x => !x.IsDeleted).ToList();
        var query = loans
        .Select(x => new LendDto
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
            ReturnEmployeeID = x.ReturnEmployeeID,
            ReturnEmployeeName = GetReturnEmployeeName(x.ReturnEmployeeID).Result,
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

    public async Task<string> GetReturnEmployeeName(Guid? id)
    {
        if (id == null)
            return "";
        return await _IdentityAcl.GetUserName(id);
    }
}
