using AppFramework.Domain;
using LMS.Contracts.Translator;
using LMS.Domain.TranslatorAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class TranslatorRepository : Repository<Translator>, ITranslatorRepository
    {
        public TranslatorRepository(BookDbContext dbContext) : base(dbContext)
        {
        }
        //public TranslatorRepository(DbContext dbContext) : base(dbContext)
        //{
        //}
    }
}

