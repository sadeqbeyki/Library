using AppFramework.Domain;
using Library.Application.DTOs.Authors;
using Library.Application.Interfaces;
using Library.Domain.Entities.AuthorAgg;
using Library.Domain.Entities.BookAgg;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance.Repositories;

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

    public async Task<Author?> GetByName(string name)
    {
        Author? author = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Name == name);
        return author;
    }
}
