using AppFramework.Domain;
using LMS.Contracts.Translator;
using LMS.Domain.BookAgg;

namespace LMS.Domain.TranslatorAgg;

public interface ITranslatorRepository : IRepository<Translator, int>
{
    Task<List<Book>> GetTranslatorBooks(int id);
    Task<List<TranslatorDto>> GetTranslators();

}

