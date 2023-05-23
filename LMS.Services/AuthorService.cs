using LMS.Contracts.Author;
using LMS.Domain.AuthorAgg;


namespace LMS.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository autherRepository)
        {
            _authorRepository = autherRepository;
        }

        public async Task<AuthorDto> Create(AuthorDto dto)
        {
            var auther = new Author
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var addAuther = await _authorRepository.AddAsync(auther);

            var result = new AuthorDto
            {
                Name = addAuther.Name,
                Description = addAuther.Description,
            };

            return result;
        }

        public Task<List<AuthorDto>> GetAll()
        {
            var result = _authorRepository.GetAll()
                .Select(author => new AuthorDto
                {
                    Id = author.Id,
                    Name = author.Name,
                    Description = author.Description,
                }).ToList();

            return Task.FromResult(result);
        }

        public async Task<AuthorDto> GetById(Guid id)
        {
            var result = await _authorRepository.GetByIdAsync(id);
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
            var result = await _authorRepository.GetByIdAsync(id);
            await _authorRepository.DeleteAsync(result);
        }

        public async Task<AuthorDto> Update(Guid id, AuthorDto entity)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);
            if (existingAuthor == null)
                return null;

            existingAuthor.Name = entity.Name;
            existingAuthor.Description = entity.Description;

            await _authorRepository.UpdateAsync(existingAuthor);
            return entity;
        }


    }
}