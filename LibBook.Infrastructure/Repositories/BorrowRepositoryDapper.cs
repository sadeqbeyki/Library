using LibBook.DomainContracts.Borrow;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

public class BorrowRepositoryDapper
{
    private readonly BookDbContext _bookDbContext;

    public BorrowRepositoryDapper(BookDbContext bookDbContext)
    {
        _bookDbContext = bookDbContext;
    }

    public async Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId)
    {
        var parameters = new[] {
                new SqlParameter("@MemberId", memberId)
            };

        var borrows = await _bookDbContext.Borrows
            .FromSqlRaw("EXEC GetBorrowsByMemberId @MemberId", parameters)
            .ToListAsync();

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
}


