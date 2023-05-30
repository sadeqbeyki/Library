using LMS.Contracts.Book;

namespace LMS.Contracts.Translator;

public interface ITranslatorService
{
    Task<TranslatorDto> GetTranslator(Guid id);
    Task<List<TranslatorDto>> GetAllTranslators();
    Task<TranslatorDto> AddTranslator(TranslatorDto entity);
    Task DeleteTranslator(Guid id);
    Task<TranslatorDto> UpdateTranslator(Guid id, TranslatorDto entity);
    Task<List<BookDto>> GetTranslatorBooks(Guid id);

}
