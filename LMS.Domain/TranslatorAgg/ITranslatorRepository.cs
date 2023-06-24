using AppFramework.Domain;
using LMS.Contracts.Author;
using LMS.Contracts.Translator;
using LMS.Domain.BookAgg;

namespace LMS.Domain.TranslatorAgg;

public interface ITranslatorRepository : IRepository<Translator>
{
    Task<List<Book>> GetTranslatorBooks(Guid id);
    Task<List<TranslatorDto>> GetTranslators();

}

