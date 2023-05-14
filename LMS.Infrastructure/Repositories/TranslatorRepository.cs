using AppFramework.Domain;
using BMS.Domain.TranslatorAgg;
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

