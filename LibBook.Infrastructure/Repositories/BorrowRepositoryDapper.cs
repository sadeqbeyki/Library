using AutoMapper.Execution;
using LibBook.DomainContracts.Borrow;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;

namespace LibBook.Infrastructure.Repositories;

public class BorrowRepositoryDapper
{
    private readonly BookDbContext _bookDbContext;


    public BorrowRepositoryDapper(BookDbContext bookDbContext)
    {
        _bookDbContext = bookDbContext;
    }

    public async Task<List<LoanDto>> GetBorrowsByEmployeeId(string employeeId)
    {
        var parameters = new[] {
                new SqlParameter("@EmployeeId", employeeId)
            };
        var borrows = await _bookDbContext.Borrows
            .FromSqlRaw("EXEC GetBorrowsByEmployeeId @EmployeeId", parameters)
            .ToListAsync();
        List<LoanDto> result = borrows.Select(b => new LoanDto
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
    public async Task<List<LoanDto>> GetBorrowsByMemberId(string memberId)
    {
        var parameters = new[] {
                new SqlParameter("@MemberId", memberId)
            };

        var borrows = await _bookDbContext.Borrows
            .FromSqlRaw("EXEC GetBorrowsByMemberId @MemberId", parameters)
            .ToListAsync();

        List<LoanDto> result = borrows.Select(b => new LoanDto
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

    public async Task<List<LoanDto>> GetOverdueBorrows()
    {
        var borrows = await _bookDbContext.Borrows.FromSqlRaw("EXEC GetOverdueBorrows").ToListAsync();

        //var result = await _bookDbContext.Database.SqlQuery<BorrowDto>("EXEC GetOverdueBorrows").ToListAsync();

        List<LoanDto> result = borrows.Select(b => new LoanDto
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

}


