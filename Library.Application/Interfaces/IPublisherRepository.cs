using AppFramework.Domain;
using Library.Application.DTOs.Publishers;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.PublisherAgg;

namespace Library.Application.Interfaces;

public interface IPublisherRepository : IRepository<Publisher, int>
{
    Task<List<PublisherDto>> GetPublishers();
    Task<List<Book>> GetPublisherBooks(int id);
    Task<Publisher> GetByName(string name);
}
