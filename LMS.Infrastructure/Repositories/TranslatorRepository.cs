using AppFramework.Domain;
using LMS.Contracts.Translator;
using LMS.Domain.BookAgg;
using LMS.Domain.TranslatorAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class TranslatorRepository : Repository<Translator>, ITranslatorRepository
    {
        private readonly BookDbContext _bookDbContext;
        public TranslatorRepository(BookDbContext dbContext) : base(dbContext)
        {
            _bookDbContext = dbContext;
        }
        public async Task<List<Book>> GetTranslatorBooks(Guid id)
        {
            List<Book> books = await _bookDbContext.Books.Where(b => b.TranslatorId == id).ToListAsync();
            return books;
        }

    }
}

