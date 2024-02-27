using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.Translator;
using Library.Application.Interfaces;
using Library.Domain.Entities.TranslatorAgg;

namespace Library.Persistance.Services
{
    public class TranslatorService : ITranslatorService
    {
        private readonly ITranslatorRepository _translatorRepository;

        public TranslatorService(ITranslatorRepository translatorRepository)
        {
            _translatorRepository = translatorRepository;
        }

        public async Task<TranslatorDto> Create(TranslatorDto dto)
        {
            var translator = new Translator
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var addTranslator = await _translatorRepository.CreateAsync(translator);

            var result = new TranslatorDto
            {
                Name = addTranslator.Name,
                Description = addTranslator.Description,
            };

            return result;
        }

        public Task<List<TranslatorDto>> GetAll()
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

        public async Task<TranslatorDto> GetTranslator(int id)
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

        public async Task DeleteTranslator(int id)
        {
            var result = await _translatorRepository.GetByIdAsync(id);
            await _translatorRepository.DeleteAsync(result);
        }

        public async Task<TranslatorDto> UpdateTranslator(int id, TranslatorDto entity)
        {
            var existingTranslator = await _translatorRepository.GetByIdAsync(id);
            if (existingTranslator == null)
                throw new KeyNotFoundException("i couldn't find a translator with this id!");

            existingTranslator.Name = entity.Name;
            existingTranslator.Description = entity.Description;

            await _translatorRepository.UpdateAsync(existingTranslator);
            return entity;
        }

        public async Task<List<BookDto>> GetTranslatorBooks(int id)
        {
            var books = await _translatorRepository.GetTranslatorBooks(id);
            var result = books.Select(b => new BookDto
            {
                Title = b.Title,
                ISBN = b.ISBN,
                Code = b.Code,
                Description = b.Description,
                CategoryId = b.CategoryId,
            }).ToList();
            return result;
        }

        public Task<List<TranslatorDto>> GetTranslators()
        {
            return _translatorRepository.GetTranslators();
        }
    }
}