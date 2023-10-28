using AppFramework.Domain;
using AutoMapper;
using AutoMapper.Execution;
using LibBook.Domain.BorrowAgg;
using LibBook.DomainContracts.Borrow;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

public partial class BorrowRepository : Repository<Borrow, int>, IBorrowRepository
{
    private readonly BookDbContext _bookDbContext;
    private readonly IMapper _mapper;
    public BorrowRepository(BookDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _bookDbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<BorrowDto>> GetBorrowsByEmployeeId(string employeeId)
    {
        var borrows = await _bookDbContext.Borrows.Where(x => x.EmployeeId == employeeId).ToListAsync();
        List<BorrowDto> result = borrows.Select(b => new BorrowDto
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

    public async Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId)
    {
        var borrows = await _bookDbContext.Borrows.Where(x => x.MemberID == memberId).ToListAsync();
        List<BorrowDto> result = borrows.Select(b => new BorrowDto
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

    public async Task<List<BorrowDto>> GetOverdueBorrows()
    {
        var borrows = await _bookDbContext.Borrows
            .Where(b => b.ReturnDate == null && b.IdealReturnDate < DateTime.Now).ToListAsync();

        List<BorrowDto> result = borrows.Select(b => new BorrowDto
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

    public void SoftDelete(int lendId)
    {
        var borrow = _bookDbContext.Borrows.FirstOrDefault(lid => lid.Id == lendId);
        if (borrow != null && borrow.IsDeleted == false)
            borrow.IsDeleted = true;
    }
}
