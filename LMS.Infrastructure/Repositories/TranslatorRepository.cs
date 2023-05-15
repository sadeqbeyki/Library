using AppFramework.Domain;
using LMS.Domain.TranslatorAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class TranslatorRepository : Repository<Translator>, ITranslatorRepository
    {
        public TranslatorRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}

