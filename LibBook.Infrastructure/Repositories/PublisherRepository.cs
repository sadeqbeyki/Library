using AppFramework.Domain;
using LibBook.Domain.PublisherAgg;
using LibBook.DomainContracts.Publisher;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories;

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
}
