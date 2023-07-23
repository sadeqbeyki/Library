using AppFramework.Domain;
using LibBook.DomainContracts.Publisher;

namespace LibBook.Domain.PublisherAgg;

public interface IPublisherRepository : IRepository<Publisher, int>
{
    Task<List<PublisherDto>> GetPublishers();
}
