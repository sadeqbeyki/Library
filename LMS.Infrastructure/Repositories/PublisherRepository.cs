using AppFramework.Domain;
using BMS.Domain.PublisherAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class PublisherRepository : Repository<Publisher>, IPublisherRepository
{
    public PublisherRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
