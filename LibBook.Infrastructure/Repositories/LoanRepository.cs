using AppFramework.Domain;
using LibBook.Domain.BorrowAgg;
using LibBook.DomainContracts.Borrow;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

public partial class LoanRepository : Repository<Borrow, int>, ILoanRepository
{
    private readonly BookDbContext _bookDbContext;
    public LoanRepository(BookDbContext dbContext) : base(dbContext)
    {
        _bookDbContext = dbContext;
    }

    public async Task<List<LoanDto>> GetBorrowsByEmployeeId(string employeeId)
    {
        var borrows = await _bookDbContext.Borrows.Where(x => x.EmployeeId == employeeId).ToListAsync();
        List<LoanDto> result = borrows.Select(b => new LoanDto
        {
            Id = b.Id,
            BookId = b.BookId,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            BorrowDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeId = b.ReturnEmployeeID,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetBorrowsByMemberId(string memberId)
    {
        var borrows = await _bookDbContext.Borrows.Where(x => x.MemberID == memberId).ToListAsync();
        List<LoanDto> result = borrows.Select(b => new LoanDto
        {
            Id = b.Id,
            BookId = b.BookId,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            BorrowDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeId = b.ReturnEmployeeID,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetOverdueLones()
    {
        var borrows = await _bookDbContext.Borrows
            .Where(b => b.ReturnDate == null && b.IdealReturnDate < DateTime.Now).ToListAsync();

        List<LoanDto> result = borrows.Select(b => new LoanDto
        {
            Id = b.Id,
            BookId = b.BookId,
            MemberId = b.MemberID,
            EmployeeId = b.EmployeeId,
            BorrowDate = b.CreationDate,
            IdealReturnDate = b.IdealReturnDate,
            ReturnEmployeeId = b.ReturnEmployeeID,
            ReturnDate = b.ReturnDate,
            Description = b.Description
        }).ToList();
        return result;
    }
}
