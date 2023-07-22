using AppFramework.Domain;
using LMS.Contracts.Author;
using LMS.Domain.AuthorAgg;
using LMS.Domain.BookAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class AuthorRepository : Repository<Author, int>, IAuthorRepository
{
    private readonly BookDbContext _dbContext;
    public AuthorRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetAuthorBooks(int id)
    {
        List<Book> books = await _dbContext.Books.Where(b => b.AuthorId == id).ToListAsync();
        return  books;
    }

    public async Task<List<AuthorDto>> GetAuthors()
    {
        return await _dbContext.Authors.Select(x => new AuthorDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
    }
}
