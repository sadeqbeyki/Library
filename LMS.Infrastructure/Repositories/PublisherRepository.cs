using AppFramework.Domain;
using LMS.Contracts.Publisher;
using LMS.Domain.PublisherAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class PublisherRepository : Repository<Publisher, int>, IPublisherRepository
{
    private readonly BookDbContext _dbContext;
    public PublisherRepository(BookDbContext bookDbContext) : base(bookDbContext)
    {
        _dbContext = bookDbContext;
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
