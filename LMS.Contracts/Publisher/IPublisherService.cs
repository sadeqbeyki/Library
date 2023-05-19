namespace LMS.Contracts.Publisher;

public interface IPublisherService
{
    Task<PublisherDto> GetById(Guid id);
    Task<List<PublisherDto>> GetAll();
    Task<PublisherDto> Create(PublisherDto entity);
    Task Delete(Guid id);
    Task<PublisherDto> Update(Guid id, PublisherDto entity);
}
