using AppFramework.Domain;
using AutoMapper;
using LibBook.Domain.BorrowAgg;
using LibBook.DomainContracts.Borrow;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

public class BorrowRepository : Repository<Borrow, int>, IBorrowRepository
{
    private readonly BookDbContext _bookDbContext;
    private readonly IMapper _mapper;
    public BorrowRepository(BookDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _bookDbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId)
    {
        var borrows =  await _bookDbContext.Borrows.Where(x => x.MemberID == memberId).ToListAsync();
        List<BorrowDto> result = borrows.Select(b => new BorrowDto
        {
            Id = b.Id,
            BookId=b.BookId,
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
