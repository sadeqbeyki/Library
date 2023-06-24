using AppFramework.Domain;
using LMS.Contracts.Author;
using LMS.Contracts.Translator;
using LMS.Domain.BookAgg;
using LMS.Domain.TranslatorAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class TranslatorRepository : Repository<Translator>, ITranslatorRepository
    {
        private readonly BookDbContext _dbContext;
        public TranslatorRepository(BookDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Book>> GetTranslatorBooks(Guid id)
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
    }
}

