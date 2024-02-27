using AppFramework.Domain;
using Library.Application.DTOs.Publisher;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.PublisherAgg;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance.Repositories;

public class PublisherRepository : Repository<Publisher, int>, IPublisherRepository
{
    private readonly BookDbContext _dbContext;
    public PublisherRepository(BookDbContext bookDbContext) : base(bookDbContext)
    {
        _dbContext = bookDbContext;
    }

    public async Task<Publisher> GetByName(string name)
    {
        return await _dbContext.Publishers.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<PublisherDto>> GetPublishers()
    {

        return await _dbContext.Publishers.Select(x => new PublisherDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();

    }

    public async Task<List<Book>> GetPublisherBooks(int id)
    {
        var publisherBooks = await _dbContext.PublisherBooks.Select(b => b.Book).ToListAsync();
        return publisherBooks;
    }
}
