using AppFramework.Domain;
using LibBook.Domain.TranslatorAgg;
using LibBook.DomainContracts.Publisher;

namespace LibBook.Domain.PublisherAgg;

public interface IPublisherRepository : IRepository<Publisher, int>
{
    Task<List<PublisherDto>> GetPublishers();

    Task<Publisher> GetByName(string name);
}
