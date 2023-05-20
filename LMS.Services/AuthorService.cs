using LMS.Contracts.Author;
using LMS.Domain.AuthorAgg;


namespace LMS.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _autherRepository;

        public AuthorService(IAuthorRepository autherRepository)
        {
            _autherRepository = autherRepository;
        }

        public async Task<AuthorDto> Create(AuthorDto dto)
        {
            var auther = new Author
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var addAuther = await _autherRepository.AddAsync(auther);

            var result = new AuthorDto
            {
                Name = addAuther.Name,
                Description = addAuther.Description,
            };

            return result;
        }

        public Task<List<AuthorDto>> GetAll()
        {
            var result = _autherRepository.GetAll()
                .Select(auther => new AuthorDto
                {
                    Id = auther.Id,
                    Name = auther.Name,
                    Description = auther.Description,
                }).ToList();

            return Task.FromResult(result);
        }

        public async Task<AuthorDto> GetById(Guid id)
        {
            var result = await _autherRepository.GetByIdAsync(id);
            AuthorDto dto = new()
            {
                Id = id,
                Name = result.Name,
                Description = result.Description,
            };
            return dto;
        }

        public async Task Delete(Guid id)
        {
            var result = await _autherRepository.GetByIdAsync(id);
            await _autherRepository.DeleteAsync(result);
        }

        public async Task<AuthorDto> Update(Guid id, AuthorDto entity)
        {
            var existingAuther = await _autherRepository.GetByIdAsync(id);
            if (existingAuther == null)
                return null;

            existingAuther.Name = entity.Name;
            existingAuther.Description = entity.Description;

            await _autherRepository.UpdateAsync(existingAuther);
            return entity;
        }


    }
}