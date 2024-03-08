using Library.Application.DTOs.Books;
using Library.Application.DTOs.Translator;

namespace Library.Application.Contracts;

public interface ITranslatorService
{
    Task<TranslatorDto> GetTranslator(int id);
    Task<List<TranslatorDto>> GetAll();
    Task<TranslatorDto> Create(TranslatorDto entity);
    Task DeleteTranslator(int id);
    Task<TranslatorDto> UpdateTranslator(int id, TranslatorDto entity);
    Task<List<BookDto>> GetTranslatorBooks(int id);
    Task<List<TranslatorDto>> GetTranslators();


}
