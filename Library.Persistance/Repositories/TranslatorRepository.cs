using AppFramework.Domain;
using Library.Application.DTOs.Translator;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.TranslatorAgg;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance.Repositories;

public class TranslatorRepository : Repository<Translator, int>, ITranslatorRepository
{
    private readonly BookDbContext _dbContext;
    public TranslatorRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetTranslatorBooks(int id)
    {
        var translatorBooks = await _dbContext.TranslatorBooks.Select(b => b.Book).ToListAsync();
        return translatorBooks;
    }

    public async Task<List<TranslatorDto>> GetTranslators()
    {
        return await _dbContext.Translators.Select(x => new TranslatorDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
    }

    public async Task<Translator> GetByName(string name)
    {
        return await _dbContext.Translators.FirstOrDefaultAsync(x => x.Name == name);
    }
}

