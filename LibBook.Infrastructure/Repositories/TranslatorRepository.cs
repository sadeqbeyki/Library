using AppFramework.Domain;
using LibBook.Domain.BookAgg;
using LibBook.Domain.TranslatorAgg;
using LibBook.DomainContracts.Translator;
using Microsoft.EntityFrameworkCore;

namespace LibBook.Infrastructure.Repositories
{
    public class TranslatorRepository : Repository<Translator, int>, ITranslatorRepository
    {
        private readonly BookDbContext _dbContext;
        public TranslatorRepository(BookDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Book>> GetTranslatorBooks(int id)
        {
            List<Book> books = await _dbContext.Books.Where(b => b.TranslatorId == id).ToListAsync();
            return books;
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
}

