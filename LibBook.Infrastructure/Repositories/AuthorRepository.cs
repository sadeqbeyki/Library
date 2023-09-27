using AppFramework.Domain;
using LibBook.Domain;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Author;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

public class AuthorRepository : Repository<Author, int>, IAuthorRepository
{
    private readonly BookDbContext _dbContext;
    public AuthorRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetAuthorBooks(int id)
    {
        var authorBooks = await _dbContext.AuthorBooks.Select(b => b.Book).ToListAsync();
        return authorBooks;
    }

    public async Task<List<AuthorDto>> GetAuthors()
    {
        return await _dbContext.Authors.Select(x => new AuthorDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
    }

    public async Task<Author> GetByName(string name)
    {
        return await _dbContext.Authors.FirstOrDefaultAsync(x => x.Name == name);
    }
}
