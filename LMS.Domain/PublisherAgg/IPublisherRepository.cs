using AppFramework.Domain;
using LMS.Contracts.Publisher;

namespace LMS.Domain.PublisherAgg;

public interface IPublisherRepository : IRepository<Publisher>
{
    Task<List<PublisherDto>> GetPublishers();
}
