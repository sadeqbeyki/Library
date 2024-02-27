using Library.Application.DTOs.Lend;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance.Repositories;

public class BorrowRepositoryDapper
{
    private readonly BookDbContext _bookDbContext;


    public BorrowRepositoryDapper(BookDbContext bookDbContext)
    {
        _bookDbContext = bookDbContext;
    }

    public async Task<List<LendDto>> GetLoansByEmployeeId(string employeeId)
    {
        var parameters = new[] {
                new SqlParameter("@EmployeeId", employeeId)
            };
        var loans = await _bookDbContext.Loans
            .FromSqlRaw("EXEC GetLoansByEmployeeId @EmployeeId", parameters)
            .ToListAsync();
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
    public async Task<List<LendDto>> GetLoansByMemberId(string memberId)
    {
        var parameters = new[] {
                new SqlParameter("@MemberId", memberId)
            };

        var loans = await _bookDbContext.Loans
            .FromSqlRaw("EXEC GetLoansByMemberId @MemberId", parameters)
            .ToListAsync();

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

    public async Task<List<LendDto>> GetOverdueLoans()
    {
        var loans = await _bookDbContext.Loans.FromSqlRaw("EXEC GetOverdueLoans").ToListAsync();

        //var result = await _bookDbContext.Database.SqlQuery<BorrowDto>("EXEC GetOverdueLoans").ToListAsync();

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

}


