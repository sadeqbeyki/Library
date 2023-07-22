using AppFramework.Domain;
using LMS.Contracts.Publisher;

namespace LMS.Domain.PublisherAgg;

public interface IPublisherRepository : IRepository<Publisher, int>
{
    Task<List<PublisherDto>> GetPublishers();
}
