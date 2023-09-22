using AppFramework.Domain;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Translator;

namespace LibBook.Domain.TranslatorAgg;

public interface ITranslatorRepository : IRepository<Translator, int>
{
    Task<List<Book>> GetTranslatorBooks(int id);
    Task<List<TranslatorDto>> GetTranslators();

    Task<Translator> GetByName(string name);


}

