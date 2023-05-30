using LMS.Contracts.Book;
using LMS.Contracts.Translator;
using LMS.Domain.TranslatorAgg;

namespace LMS.Services
{
    public class TranslatorService : ITranslatorService
    {
        private readonly ITranslatorRepository _translatorRepository;

        public TranslatorService(ITranslatorRepository translatorRepository)
        {
            _translatorRepository = translatorRepository;
        }

        public async Task<TranslatorDto> AddTranslator(TranslatorDto dto)
        {
            var translator = new Translator
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var addTranslator = await _translatorRepository.AddAsync(translator);

            var result = new TranslatorDto
            {
                Name = addTranslator.Name,
                Description = addTranslator.Description,
            };

            return result;
        }

        public Task<List<TranslatorDto>> GetAllTranslators()
        {
            var result = _translatorRepository.GetAll()
                .Select(translator => new TranslatorDto
                {
                    Id = translator.Id,
                    Name = translator.Name,
                    Description = translator.Description,
                }).ToList();

            return Task.FromResult(result);
        }

        public async Task<TranslatorDto> GetTranslator(Guid id)
        {
            var result = await _translatorRepository.GetByIdAsync(id);
            TranslatorDto dto = new()
            {
                Id = id,
                Name = result.Name,
                Description = result.Description,
            };
            return dto;
        }

        public async Task DeleteTranslator(Guid id)
        {
            var result = await _translatorRepository.GetByIdAsync(id);
            await _translatorRepository.DeleteAsync(result);
        }

        public async Task<TranslatorDto> UpdateTranslator(Guid id, TranslatorDto entity)
        {
            var existingTranslator = await _translatorRepository.GetByIdAsync(id);
            if (existingTranslator == null)
                throw new  KeyNotFoundException("i couldn't find a translator with this id!");

            existingTranslator.Name = entity.Name;
            existingTranslator.Description = entity.Description;

            await _translatorRepository.UpdateAsync(existingTranslator);
            return entity;
        }

        public async Task<List<BookDto>> GetTranslatorBooks(Guid id)
        {
            var books = await _translatorRepository.GetTranslatorBooks(id);
            var result = books.Select(b => new BookDto
            {
                Title = b.Title,
                ISBN = b.ISBN,
                Code = b.Code,
                Description = b.Description,
                PublisherId = b.PublisherId,
                AuthorId = b.AuthorId,
                TranslatorId = b.TranslatorId,
                CategoryId = b.CategoryId,
            }).ToList();
            return result;
        }
    }
}