using AppFramework.Domain;
using BMS.Domain.TranslatorAgg;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    public class TranslatorRepository : Repository<Translator>
    {
        public TranslatorRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}

