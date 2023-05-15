using AppFramework.Domain;
using LMS.Domain.PublisherAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class PublisherRepository : Repository<Publisher>, IPublisherRepository
{
    public PublisherRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
