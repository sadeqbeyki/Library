using LMS.Contracts.Publisher;
using LMS.Domain.PublisherAgg;


namespace LMS.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public async Task<PublisherDto> Create(PublisherDto dto)
        {
            var publisher = new Publisher
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var addPublisher = await _publisherRepository.CreateAsync(publisher);

            var result = new PublisherDto
            {
                Name = addPublisher.Name,
                Description = addPublisher.Description,
            };

            return result;
        }

        public Task<List<PublisherDto>> GetAll()
        {
            var result = _publisherRepository.GetAll()
                .Select(publisher => new PublisherDto
                {
                    Id = publisher.Id,
                    Name = publisher.Name,
                    Description = publisher.Description,
                }).ToList();

            return Task.FromResult(result);
        }

        public async Task<PublisherDto> GetById(int id)
        {
            var result = await _publisherRepository.GetByIdAsync(id);
            PublisherDto dto = new()
            {
                Id = id,
                Name = result.Name,
                Description = result.Description,
            };
            return dto;
        }

        public async Task Delete(int id)
        {
            var result = await _publisherRepository.GetByIdAsync(id);
            await _publisherRepository.DeleteAsync(result);
        }

        public async Task<PublisherDto> Update(int id, PublisherDto entity)
        {
            var existingPublisher = await _publisherRepository.GetByIdAsync(id);
            if (existingPublisher == null)
                return null;

            existingPublisher.Name = entity.Name;
            existingPublisher.Description = entity.Description;

            await _publisherRepository.UpdateAsync(existingPublisher);
            return entity;
        }

        public Task<List<PublisherDto>> GetPublishers()
        {
            return _publisherRepository.GetPublishers();
        }
    }
}