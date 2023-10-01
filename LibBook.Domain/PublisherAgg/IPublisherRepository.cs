using AppFramework.Domain;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Publisher;

namespace LibBook.Domain.PublisherAgg;

public interface IPublisherRepository : IRepository<Publisher, int>
{
    Task<List<PublisherDto>> GetPublishers();
    Task<List<Book>> GetPublisherBooks(int id);
    Task<Publisher> GetByName(string name);
}
