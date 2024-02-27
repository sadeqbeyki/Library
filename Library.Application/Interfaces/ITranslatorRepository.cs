using AppFramework.Domain;
using Library.Application.DTOs.Translator;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.TranslatorAgg;

namespace Library.Application.Interfaces;

public interface ITranslatorRepository : IRepository<Translator, int>
{
    Task<List<Book>> GetTranslatorBooks(int id);
    Task<List<TranslatorDto>> GetTranslators();

    Task<Translator> GetByName(string name);


}

