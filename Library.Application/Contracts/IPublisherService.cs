using Library.Application.DTOs.Publisher;

namespace Library.Application.Contracts;

public interface IPublisherService
{
    Task<PublisherDto> GetById(int id);
    Task<List<PublisherDto>> GetAll();
    Task<PublisherDto> Create(PublisherDto entity);
    Task Delete(int id);
    Task<PublisherDto> Update(int id, PublisherDto entity);
    Task<List<PublisherDto>> GetPublishers();

}
